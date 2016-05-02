// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they bagin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using HtmlRenderer.Entities;
using HtmlRenderer.Parse;
using HtmlRenderer.Utils;

namespace HtmlRenderer.Dom
{
    /// <summary>
    /// Represents a CSS Box of text or replaced elements.
    /// </summary>
    /// <remarks>
    /// The Box can contains other boxes, that's the way that the CSS Tree
    /// is composed.
    /// 
    /// To know more about boxes visit CSS spec:
    /// http://www.w3.org/TR/CSS21/box.html
    /// </remarks>
    internal class CssBox : CssBoxProperties, IDisposable
    {
        #region Fields and Consts

        /// <summary>
        /// the parent css box of this css box in the hierarchy
        /// </summary>
        private CssBox _parentBox;
        
        /// <summary>
        /// the root container for the hierarchy
        /// </summary>
        protected HtmlContainer _htmlContainer;

        /// <summary>
        /// the html tag that is associated with this css box, null if anonymous box
        /// </summary>
        private readonly HtmlTag _htmltag;

        private readonly List<CssRect> _boxWords = new List<CssRect>();
        private readonly List<CssBox> _boxes = new List<CssBox>();
        private readonly List<CssLineBox> _lineBoxes = new List<CssLineBox>();
        private readonly List<CssLineBox> _parentLineBoxes = new List<CssLineBox>();
        private readonly Dictionary<CssLineBox, RectangleF> _rectangles = new Dictionary<CssLineBox, RectangleF>();

        /// <summary>
        /// the inner text of the box
        /// </summary>
        private SubString _text;

        /// <summary>
        /// Do not use or alter this flag
        /// </summary>
        /// <remarks>
        /// Flag that indicates that CssTable algorithm already made fixes on it.
        /// </remarks>
        internal bool _tableFixed;

        protected bool _wordsSizeMeasured;
        private CssBox _listItemBox;
        private CssLineBox _firstHostingLineBox;
        private CssLineBox _lastHostingLineBox;

        /// <summary>
        /// handler for loading background image
        /// </summary>
        private ImageLoadHandler _imageLoadHandler;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="parentBox">optional: the parent of this css box in html</param>
        /// <param name="tag">optional: the html tag associated with this css box</param>
        public CssBox(CssBox parentBox, HtmlTag tag)
        {
            if(parentBox != null)
            {
                _parentBox = parentBox;
                _parentBox.Boxes.Add(this);
            }
            _htmltag = tag;
        }

        /// <summary>
        /// Gets the HtmlContainer of the Box.
        /// WARNING: May be null.
        /// </summary>
        public HtmlContainer HtmlContainer
        {
            get { return _htmlContainer ?? (_parentBox != null ? _parentBox.HtmlContainer : null); }
            set { _htmlContainer = value; }
        }

        /// <summary>
        /// Gets or sets the parent box of this box
        /// </summary>
        public CssBox ParentBox
        {
            get { return _parentBox; }
            set
            {
                //Remove from last parent
                if (_parentBox != null && _parentBox.Boxes.Contains(this))
                {
                    _parentBox.Boxes.Remove(this);
                }

                _parentBox = value;

                //Add to new parent
                if (value != null && !value.Boxes.Contains(this))
                {
                    _parentBox.Boxes.Add(this);
                    _htmlContainer = value.HtmlContainer;
                }
            }
        }

        /// <summary>
        /// Gets the childrenn boxes of this box
        /// </summary>
        public List<CssBox> Boxes
        {
            get { return _boxes; }
        }

        /// <summary>
        /// is the box "Display" is "Inline", is this is an inline box and not block.
        /// </summary>
        public bool IsInline
        {
            get { return Display == CssConstants.Inline; }
        }

        /// <summary>
        /// is the box "Display" is "Block", is this is an block box and not inline.
        /// </summary>
        public bool IsBlock
        {
            get { return Display == CssConstants.Block || Display == CssConstants.InlineBlock; }
        }

        /// <summary>
        /// Is the css box clickable (by default only "a" element is clickable)
        /// </summary>
        public virtual bool IsClickable
        {
            get { return HtmlTag != null && HtmlTag.Name == HtmlConstants.A; }
        }

        /// <summary>
        /// Get the href link of the box (by default get "href" attribute)
        /// </summary>
        public virtual string HrefLink
        {
            get { return GetAttribute(HtmlConstants.Href); }
        }

        /// <summary>
        /// Gets the containing block-box of this box. (The nearest parent box with display=block)
        /// </summary>
        public CssBox ContainingBlock
        {
            get
            {
                if (ParentBox == null)
                {
                    return this; //This is the initial containing block.
                }

                var box = ParentBox;
                while (!box.IsBlock &&
                        box.Display != CssConstants.Table &&
                        box.Display != CssConstants.TableCell &&
                        box.ParentBox != null)
                {
                    box = box.ParentBox;
                }

                //Comment this following line to treat always superior box as block
                if (box == null) 
                    throw new Exception("There's no containing block on the chain");

                return box;
            }
        }

        /// <summary>
        /// Gets the HTMLTag that hosts this box
        /// </summary>
        public HtmlTag HtmlTag
        {
            get { return _htmltag; }
        }

        /// <summary>
        /// Gets if this box represents an image
        /// </summary>
        public bool IsImage
        {
            get { return Words.Count == 1 && Words[0].IsImage; } 
        }

        /// <summary>
        /// Tells if the box is empty or contains just blank spaces
        /// </summary>
        public bool IsSpaceOrEmpty
        {
            get 
            {
                if ((Words.Count != 0 || Boxes.Count != 0) && (Words.Count != 1 || !Words[0].IsSpaces))
                {
                    foreach (CssRect word in Words)
                    {
                        if (!word.IsSpaces)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the inner text of the box
        /// </summary>
        public SubString Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _boxWords.Clear();
            }
        }

        /// <summary>
        /// Gets the line-boxes of this box (if block box)
        /// </summary>
        internal List<CssLineBox> LineBoxes
        {
            get { return _lineBoxes; }
        }

        /// <summary>
        /// Gets the linebox(es) that contains words of this box (if inline)
        /// </summary>
        internal List<CssLineBox> ParentLineBoxes
        {
            get { return _parentLineBoxes; }
        }

        /// <summary>
        /// Gets the rectangles where this box should be painted
        /// </summary>
        internal Dictionary<CssLineBox, RectangleF> Rectangles
        {
            get { return _rectangles; }
        }

        /// <summary>
        /// Gets the BoxWords of text in the box
        /// </summary>
        internal List<CssRect> Words
        {
            get { return _boxWords; }
        }

        /// <summary>
        /// Gets the first word of the box
        /// </summary>
        internal CssRect FirstWord
        {
            get { return Words[0]; }
        }

        /// <summary>
        /// Gets or sets the first linebox where content of this box appear
        /// </summary>
        internal CssLineBox FirstHostingLineBox
        {
            get { return _firstHostingLineBox; }
            set { _firstHostingLineBox = value; }
        }

        /// <summary>
        /// Gets or sets the last linebox where content of this box appear
        /// </summary>
        internal CssLineBox LastHostingLineBox
        {
            get { return _lastHostingLineBox; }
            set { _lastHostingLineBox = value; }
        }

        /// <summary>
        /// Create new css box for the given parent with the given html tag.<br/>
        /// </summary>
        /// <param name="tag">the html tag to define the box</param>
        /// <param name="parent">the box to add the new box to it as child</param>
        /// <returns>the new box</returns>
        public static CssBox CreateBox(HtmlTag tag, CssBox parent = null)
        {
            ArgChecker.AssertArgNotNull(tag, "tag");

            if(tag.Name == HtmlConstants.Img)
            {
                return new CssBoxImage(parent, tag);
            }
            else if (tag.Name == HtmlConstants.Iframe)
            {
                return new CssBoxFrame(parent, tag);
            }
            else if (tag.Name == HtmlConstants.Hr)
            {
                return new CssBoxHr(parent, tag);
            }
            else
            {
                return new CssBox(parent, tag);                
            }
        }

        /// <summary>
        /// Create new css box for the given parent with the given optional html tag and insert it either
        /// at the end or before the given optional box.<br/>
        /// If no html tag is given the box will be anonymous.<br/>
        /// If no before box is given the new box will be added at the end of parent boxes collection.<br/>
        /// If before box doesn't exists in parent box exception is thrown.<br/>
        /// </summary>
        /// <remarks>
        /// To learn more about anonymous inline boxes visit: http://www.w3.org/TR/CSS21/visuren.html#anonymous
        /// </remarks>
        /// <param name="parent">the box to add the new box to it as child</param>
        /// <param name="tag">optional: the html tag to define the box</param>
        /// <param name="before">optional: to insert as specific location in parent box</param>
        /// <returns>the new box</returns>
        public static CssBox CreateBox(CssBox parent, HtmlTag tag = null, CssBox before = null)
        {
            ArgChecker.AssertArgNotNull(parent, "parent");

            var newBox = new CssBox(parent, tag);
            newBox.InheritStyle();
            if (before != null)
            {
                newBox.SetBeforeBox(before);
            }
            return newBox;
        }

        /// <summary>
        /// Create new css block box.
        /// </summary>
        /// <returns>the new block box</returns>
        public static CssBox CreateBlock()
        {
            var box = new CssBox(null, null);
            box.Display = CssConstants.Block;
            return box;
        }

        /// <summary>
        /// Create new css block box for the given parent with the given optional html tag and insert it either
        /// at the end or before the given optional box.<br/>
        /// If no html tag is given the box will be anonymous.<br/>
        /// If no before box is given the new box will be added at the end of parent boxes collection.<br/>
        /// If before box doesn't exists in parent box exception is thrown.<br/>
        /// </summary>
        /// <remarks>
        /// To learn more about anonymous block boxes visit CSS spec:
        /// http://www.w3.org/TR/CSS21/visuren.html#anonymous-block-level
        /// </remarks>
        /// <param name="parent">the box to add the new block box to it as child</param>
        /// <param name="tag">optional: the html tag to define the box</param>
        /// <param name="before">optional: to insert as specific location in parent box</param>
        /// <returns>the new block box</returns>
        public static CssBox CreateBlock(CssBox parent, HtmlTag tag = null, CssBox before = null)
        {
            ArgChecker.AssertArgNotNull(parent, "parent");

            var newBox = CreateBox(parent, tag, before);
            newBox.Display = CssConstants.Block;
            return newBox;
        }

        /// <summary>
        /// Measures the bounds of box and children, recursively.<br/>
        /// Performs layout of the DOM structure creating lines by set bounds restrictions.
        /// </summary>
        /// <param name="g">Device context to use</param>
        public void PerformLayout(Graphics g)
        {
            try
            {
                PerformLayoutImp(g);
            }
            catch (Exception ex)
            {
                HtmlContainer.ReportError(HtmlRenderErrorType.Layout, "Exception in box layout", ex);                
            }
        }

        /// <summary>
        /// Paints the fragment
        /// </summary>
        /// <param name="g">Device context to use</param>
        public void Paint(Graphics g)
        {
            try
            {
                if (Display != CssConstants.None && Visibility == CssConstants.Visible)
                {
                    PaintImp(g);
                }
            }
            catch (Exception ex)
            {
                HtmlContainer.ReportError(HtmlRenderErrorType.Paint, "Exception in box paint", ex);
            }
        }

        /// <summary>
        /// Set this box in 
        /// </summary>
        /// <param name="before"></param>
        public void SetBeforeBox(CssBox before)
        {
            int index = _parentBox.Boxes.IndexOf(before);
            if (index < 0)
                throw new Exception("before box doesn't exist on parent");

            _parentBox.Boxes.Remove(this);
            _parentBox.Boxes.Insert(index, this);
        }

        /// <summary>
        /// Splits the text into words and saves the result
        /// </summary>
        public void ParseToWords()
        {
            _boxWords.Clear();

            if(WhiteSpace == CssConstants.Pre)
            {
                // preformated text needs to preserve white-spaces splitting by new-line and create a word for the new-lines.
                int startIdx = 0;
                while (startIdx < _text.Length)
                {
                    while (startIdx < _text.Length && _text[startIdx] == '\r')
                        startIdx++;

                    var endIdx = startIdx;
                    while (endIdx < _text.Length && _text[endIdx] != '\n')
                        endIdx++;

                    if (startIdx < _text.Length)
                    {
                        if(endIdx > startIdx)
                        {
                            var word = HtmlUtils.DecodeHtml(_text.Substring(startIdx, endIdx - startIdx));
                            _boxWords.Add(new CssRectWord(this, word, false, false));
                        }

                        // create new-line word so it will effect the layout
                        if (endIdx < _text.Length)
                            _boxWords.Add(new CssRectWord(this, "\n", false, false));
                    }
                    startIdx = endIdx + 1;
                }
            }
            else
            {
                // regular text ignores all white-space charecters (including new-lines) and split by them.
                int startIdx = 0;
                while (startIdx < _text.Length)
                {
                    while (startIdx < _text.Length && char.IsWhiteSpace(_text[startIdx]))
                        startIdx++;

                    var endIdx = startIdx + 1;
                    while (endIdx < _text.Length && !char.IsWhiteSpace(_text[endIdx]) && _text[endIdx] != '-')
                        endIdx++;

                    int idxInc = 0;
                    if (endIdx < _text.Length && _text[endIdx] == '-')
                        endIdx++;
                    else
                        idxInc = 1;

                    if (startIdx < _text.Length)
                    {
                        var hasSpaceBefore = startIdx > 0 && _boxWords.Count == 0 && char.IsWhiteSpace(_text[startIdx - 1]);
                        var hasSpaceAfter = endIdx < _text.Length && char.IsWhiteSpace(_text[endIdx]);
                        var word = HtmlUtils.DecodeHtml(_text.Substring(startIdx, endIdx - startIdx));
                        _boxWords.Add(new CssRectWord(this, word, hasSpaceBefore, hasSpaceAfter));
                    }
                    startIdx = endIdx + idxInc;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (_imageLoadHandler != null)
                _imageLoadHandler.Dispose();

            foreach (var childBox in Boxes)
            {
                childBox.Dispose();
            }
        }

        #region Private Methods

        /// <summary>
        /// Measures the bounds of box and children, recursively.<br/>
        /// Performs layout of the DOM structure creating lines by set bounds restrictions.<br/>
        /// </summary>
        /// <param name="g">Device context to use</param>
        protected virtual void PerformLayoutImp(Graphics g)
        {
            if (Display != CssConstants.None)
            {
                RectanglesReset();
                MeasureWordsSize(g);
            }

            if (IsBlock || Display == CssConstants.ListItem || Display == CssConstants.Table || Display == CssConstants.InlineTable || Display == CssConstants.TableCell)
            {
                // Because their width and height are set by CssTable
                if (Display != CssConstants.TableCell && Display != CssConstants.Table)
                {
                    float width = ContainingBlock.Size.Width
                                  - ContainingBlock.ActualPaddingLeft - ContainingBlock.ActualPaddingRight
                                  - ContainingBlock.ActualBorderLeftWidth - ContainingBlock.ActualBorderRightWidth
                                   - ActualBorderLeftWidth - ActualBorderRightWidth;

                    if (Width != CssConstants.Auto && !string.IsNullOrEmpty(Width))
                    {
                        width = CssValueParser.ParseLength(Width, width, this);
                    }

                    Size = new SizeF(width, Size.Height);

                    // must be seperate because the margin can be calculated by percantage of the width
                    Size = new SizeF(width - ActualMarginLeft - ActualMarginRight, Size.Height);
                }

                if (Display != CssConstants.TableCell)
                {
                    var prevSibling = DomUtils.GetPreviousSibling(this);
                    float left = ContainingBlock.Location.X + ContainingBlock.ActualPaddingLeft + ActualMarginLeft + ContainingBlock.ActualBorderLeftWidth;
                    float top = (prevSibling == null && ParentBox != null ? ParentBox.ClientTop : ParentBox == null ? Location.Y : 0) + MarginTopCollapse(prevSibling) + (prevSibling != null ? prevSibling.ActualBottom + prevSibling.ActualBorderBottomWidth : 0);
                    Location = new PointF(left, top);
                    ActualBottom = top;
                }

                //If we're talking about a table here..
                if (Display == CssConstants.Table || Display == CssConstants.InlineTable)
                {
                    CssLayoutEngineTable.PerformLayout(g, this);
                }
                else
                {
                    //If there's just inlines, create LineBoxes
                    if (DomUtils.ContainsInlinesOnly(this))
                    {
                        ActualBottom = Location.Y;
                        CssLayoutEngine.CreateLineBoxes(g, this); //This will automatically set the bottom of this block
                        //ActualBottom += ActualMarginBottom;
                    }
                    else if (_boxes.Count > 0)
                    {
                        foreach (var childBox in Boxes)
                        {
                            childBox.PerformLayout(g);
                        }
                        ActualBottom = MarginBottomCollapse();
                    }
                }
            }
            else
            {
                var prevSibling = DomUtils.GetPreviousSibling(this);
                if(prevSibling != null)
                    ActualBottom = prevSibling.ActualBottom;
            }

            CreateListItemBox(g);

            var actualWidth = Math.Max(GetMinimumWidth(), Size.Width < 99999 ? Size.Width : 0);
            HtmlContainer.ActualSize = CommonUtils.Max(HtmlContainer.ActualSize, new SizeF(actualWidth, Size.Height));
        }

        /// <summary>
        /// Assigns words its width and height
        /// </summary>
        /// <param name="g"></param>
        internal virtual void MeasureWordsSize(Graphics g)
        {
            if (!_wordsSizeMeasured)
            {
                if (BackgroundImage != CssConstants.None && _imageLoadHandler == null)
                {
                    _imageLoadHandler = new ImageLoadHandler(OnImageLoadComplete);
                    _imageLoadHandler.LoadImage(HtmlContainer, BackgroundImage, HtmlTag != null ? HtmlTag.Attributes : null);
                }

                MeasureWordSpacing(g);
                
                if(Words.Count > 0)
                {
                    foreach (var boxWord in Words)
                    {
                        using(var sf = new StringFormat())
                        {
                            sf.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.MeasureTrailingSpaces;
                            sf.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, boxWord.Text.Length) });

                            var regions = g.MeasureCharacterRanges(boxWord.Text, ActualFont, RectangleF.Empty, sf);

                            SizeF s = regions[0].GetBounds(g).Size;
                            PointF p = regions[0].GetBounds(g).Location;

                            boxWord.LastMeasureOffset = new PointF(p.X, p.Y);
                            boxWord.Width = boxWord.Text != "\n" ? s.Width : 0;
                            boxWord.Height = s.Height;
                        }
                    }
                }
                
                _wordsSizeMeasured = true;
            }
        }

        /// <summary>
        /// Get the parent of this css properties instance.
        /// </summary>
        /// <returns></returns>
        protected sealed override CssBoxProperties GetParent()
        {
            return _parentBox;
        }

        /// <summary>
        /// Gets the index of the box to be used on a (ordered) list
        /// </summary>
        /// <returns></returns>
        private int GetIndexForList()
        {
            int index = 0;

            foreach (CssBox b in ParentBox.Boxes)
            {
                if (b.Display == CssConstants.ListItem) 
                    index++;
                if (b.Equals(this)) 
                    return index;
            }

            return index;
        }

        /// <summary>
        /// Creates the <see cref="_listItemBox"/>
        /// </summary>
        /// <param name="g"></param>
        private void CreateListItemBox(Graphics g)
        {
            if (Display == CssConstants.ListItem)
            {
                if (_listItemBox == null)
                {
                    _listItemBox = new CssBox(null, null);
                    _listItemBox.InheritStyle(this);
                    _listItemBox.Display = CssConstants.Inline;
                    _listItemBox._htmlContainer = HtmlContainer;

                    if (ParentBox != null && ListStyleType == CssConstants.Decimal)
                    {
                        _listItemBox.Text = new SubString(GetIndexForList().ToString(CultureInfo.InvariantCulture) + ".");
                    }
                    else
                    {
                        _listItemBox.Text = new SubString("�");
                    }
                    _listItemBox.ParseToWords();
                    
                    _listItemBox.PerformLayoutImp(g);
                    _listItemBox.Size = new SizeF(_listItemBox.Words[0].Width, _listItemBox.Words[0].Height); 
                }
                _listItemBox.Words[0].Left = Location.X - _listItemBox.Size.Width - 5;
                _listItemBox.Words[0].Top = Location.Y + ActualPaddingTop;// +FontAscent;
            }
        }

        /// <summary>
        /// Searches for the first word occourence inside the box, on the specified linebox
        /// </summary>
        /// <param name="b"></param>
        /// <param name="line"> </param>
        /// <returns></returns>
        internal CssRect FirstWordOccourence(CssBox b, CssLineBox line)
        {
            if (b.Words.Count == 0 && b.Boxes.Count == 0)
            {
                return null;
            }

            if (b.Words.Count > 0)
            {   
                foreach (CssRect word in b.Words)
                {
                    if (line.Words.Contains(word))
                    {
                        return word;
                    }
                }
                return null;
            }
            else
            {
                foreach (CssBox bb in b.Boxes)
                {
                    CssRect w = FirstWordOccourence(bb, line);

                    if (w != null)
                    {
                        return w;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the specified Attribute, returns string.Empty if no attribute specified
        /// </summary>
        /// <param name="attribute">Attribute to retrieve</param>
        /// <returns>Attribute value or string.Empty if no attribute specified</returns>
        internal string GetAttribute(string attribute)
        {
            return GetAttribute(attribute, string.Empty);
        }

        /// <summary>
        /// Gets the value of the specified attribute of the source HTML tag.
        /// </summary>
        /// <param name="attribute">Attribute to retrieve</param>
        /// <param name="defaultValue">Value to return if attribute is not specified</param>
        /// <returns>Attribute value or defaultValue if no attribute specified</returns>
        internal string GetAttribute(string attribute, string defaultValue)
        {
            return HtmlTag != null ? HtmlTag.TryGetAttribute(attribute, defaultValue) : defaultValue;
        }

        /// <summary>
        /// Gets the minimum width that the box can be.<br/>
        /// The box can be as thin as the longest word plus padding.<br/>
        /// The check is deep thru box tree.<br/>
        /// </summary>
        /// <returns>the min width of the box</returns>
        internal float GetMinimumWidth()
        {
            float maxw = 0f;
            float padding = 0f;
            CssRect word = null;

            GetMinimumWidth_LongestWord(this, ref maxw, ref word);

            if (word != null)
            {
                GetMinimumWidth_BubblePadding(word.OwnerBox, this, ref padding);
            }

            return maxw + padding;
        }

        /// <summary>
        /// Bubbles up the padding from the starting box
        /// </summary>
        /// <param name="box"></param>
        /// <param name="endbox"> </param>
        /// <param name="sum"> </param>
        /// <returns></returns>
        private static void GetMinimumWidth_BubblePadding(CssBox box, CssBox endbox, ref float sum)
        {
            //float padding = box.ActualMarginLeft + box.ActualBorderLeftWidth + box.ActualPaddingLeft +
            //    box.ActualMarginRight + box.ActualBorderRightWidth + box.ActualPaddingRight;

            float padding =  box.ActualBorderLeftWidth + box.ActualPaddingLeft +
                 box.ActualBorderRightWidth + box.ActualPaddingRight;

            sum += padding;

            if (!box.Equals(endbox))
            {
                GetMinimumWidth_BubblePadding(box.ParentBox, endbox, ref sum);
            }
        }

        /// <summary>
        /// Gets the longest word (in width) inside the box, deeply.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="maxw"> </param>
        /// <param name="word"> </param>
        /// <returns></returns>
        private void GetMinimumWidth_LongestWord(CssBox b, ref float maxw, ref CssRect word)
        {
            if (b.Words.Count > 0)
            {
                foreach (CssRect w in b.Words)
                {
                    if (w.Width > maxw)
                    {
                        maxw = w.Width;
                        word = w;
                    }
                }
            }
            else
            {
                foreach(CssBox bb in b.Boxes)
                    GetMinimumWidth_LongestWord(bb, ref maxw,ref word);
            }
        }

        /// <summary>
        /// Gets the maximum bottom of the boxes inside the startBox
        /// </summary>
        /// <param name="startBox"></param>
        /// <param name="currentMaxBottom"></param>
        /// <returns></returns>
        internal float GetMaximumBottom(CssBox startBox, float currentMaxBottom)
        {
            foreach (CssLineBox line in startBox.Rectangles.Keys)
            {
                currentMaxBottom = Math.Max(currentMaxBottom, startBox.Rectangles[line].Bottom);
            }

            foreach (CssBox b in startBox.Boxes)
            {
                currentMaxBottom = Math.Max(currentMaxBottom, b.ActualBottom);
                currentMaxBottom = Math.Max(currentMaxBottom, GetMaximumBottom(b, currentMaxBottom));
            }

            return currentMaxBottom;
        }

        /// <summary>
        /// Get the width of the box at full width (No line breaks)
        /// </summary>
        /// <returns></returns>
        internal void GetFullWidth(Graphics g, out float minWidth, out float maxWidth)
        {
            float sum = 0f;
            float min = 0f;
            float paddingsum = 0f;
            GetFullWidth_WordsWith(this, ref sum, ref min, ref paddingsum);

            maxWidth = paddingsum + sum;
            minWidth = paddingsum + (min < 99999 ? min : 0);
        }

        /// <summary>
        /// Gets the longest word (in width) inside the box, deeply.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="sum"> </param>
        /// <param name="paddingsum"> </param>
        /// <returns></returns>
        private static void GetFullWidth_WordsWith(CssBox b, ref float sum, ref float min, ref float paddingsum)
        {
            float? oldSum = null;
            if (b.Display != CssConstants.Inline)
            {
                oldSum = sum;
                sum = 0;
            }

            paddingsum += b.ActualBorderLeftWidth + b.ActualBorderRightWidth + b.ActualPaddingRight + b.ActualPaddingLeft;

            if (b.Words.Count > 0)
            {
                foreach (CssRect word in b.Words)
                {
                    sum += word.FullWidth;
                    min = Math.Max(min, word.Width);
                }

            }
            else
            {
                foreach (CssBox bb in b.Boxes)
                {
                    GetFullWidth_WordsWith(bb, ref sum, ref min, ref paddingsum);
                }
            }
            
            if (oldSum.HasValue)
            {
                sum = Math.Max(sum, oldSum.Value);
            }
        }

        /// <summary>
        /// Gets if this box has only inline siblings (including itself)
        /// </summary>
        /// <returns></returns>
        internal bool HasJustInlineSiblings()
        {
            if (ParentBox == null)
            {
                return false;
            }

            return DomUtils.ContainsInlinesOnly(ParentBox);
        }

        /// <summary>
        /// Gets the rectangles where inline box will be drawn. See Remarks for more info.
        /// </summary>
        /// <returns>Rectangles where content should be placed</returns>
        /// <remarks>
        /// Inline boxes can be splitted across different LineBoxes, that's why this method
        /// Delivers a rectangle for each LineBox related to this box, if inline.
        /// </remarks>

         /// <summary>
        /// Inherits inheritable values from parent.
        /// </summary>
        internal new void InheritStyle(CssBox box = null, bool everything = false)
        {
            base.InheritStyle(box ?? ParentBox, everything);
        }

        /// <summary>
        /// Gets the result of collapsing the vertical margins of the two boxes
        /// </summary>
        /// <param name="prevSibling">the previous box under the same parent</param>
        /// <returns>Resulting top margin</returns>
        protected float MarginTopCollapse(CssBoxProperties prevSibling)
        {
            float value;
            if (prevSibling != null )
            {
                value = Math.Max(prevSibling.ActualMarginBottom, ActualMarginTop);
                CollapsedMarginTop = value;
            }
            else if (_parentBox != null && ActualPaddingTop < 0.1 && ActualPaddingBottom < 0.1 && _parentBox.ActualPaddingTop < 0.1 && _parentBox.ActualPaddingBottom < 0.1)
            {
                value = Math.Max(0, ActualMarginTop - Math.Max(_parentBox.ActualMarginTop, _parentBox.CollapsedMarginTop));
            }
            else
            {
                value = ActualMarginTop;
            }
  
            // fix for hr tag
            if (value < 0.1 && HtmlTag != null && HtmlTag.Name == "hr")
            {
                value = GetEmHeight() * 1.1f;
            }
            
            return value;
        }

        /// <summary>
        /// Gets the result of collapsing the vertical margins of the two boxes
        /// </summary>
        /// <returns>Resulting bottom margin</returns>
        private float MarginBottomCollapse()
        {
            float margin = 0;
            if (_parentBox == null || (ParentBox.Boxes.IndexOf(this) == ParentBox.Boxes.Count - 1 && _parentBox.ActualMarginBottom < 0.1))
            {
                var lastChildBottomMargin = _boxes[_boxes.Count - 1].ActualMarginBottom;
                margin = Height == "auto" ? Math.Max(ActualMarginBottom, lastChildBottomMargin) : lastChildBottomMargin;
            }
            return Math.Max(ActualBottom, _boxes[_boxes.Count - 1].ActualBottom + margin + ActualPaddingBottom);
        }

        /// <summary>
        /// Deeply offsets the top of the box and its contents
        /// </summary>
        /// <param name="amount"></param>
        internal void OffsetTop(float amount)
        {
            List<CssLineBox> lines = new List<CssLineBox>();
            foreach (CssLineBox line in Rectangles.Keys)
                lines.Add(line);

            foreach (CssLineBox line in lines)
            {
                RectangleF r = Rectangles[line];
                Rectangles[line] = new RectangleF(r.X, r.Y + amount, r.Width, r.Height);
            }

            foreach (CssRect word in Words)
            {
                word.Top += amount;
            }
            
            foreach (CssBox b in Boxes)
            {
                b.OffsetTop(amount);
            }
            //TODO: Aqu� me quede: no se mueve bien todo (probar con las tablas rojas)
            Location = new PointF(Location.X, Location.Y + amount);
        }

        /// <summary>
        /// Paints the fragment
        /// </summary>
        /// <param name="g">the device to draw to</param>
        protected virtual void PaintImp(Graphics g)
        {
            if (Display != CssConstants.None && (Display != CssConstants.TableCell || EmptyCells != CssConstants.Hide || !IsSpaceOrEmpty))
            {
                Region prevClip = RenderUtils.ClipGraphicsByOverflow(g, this);
                
                var areas = Rectangles.Count == 0 ? new List<RectangleF>(new[] { Bounds }) : new List<RectangleF>(Rectangles.Values);

                RectangleF[] rects = areas.ToArray();
                PointF offset = HtmlContainer != null ? HtmlContainer.ScrollOffset : PointF.Empty;
                
                for (int i = 0; i < rects.Length; i++)
                {
                    var actualRect = rects[i];
                    actualRect.Offset(offset);

                    PaintBackground(g, actualRect, i == 0, i == rects.Length - 1);
                    PaintBorder(g, actualRect, i == 0, i == rects.Length - 1);
                }

                if (Words.Count > 0)
                {
                    Font font = ActualFont;
                    var brush = CssUtils.GetSolidBrush(ActualColor);
                    foreach (var word in Words)
                    {
                        if (word.Selected)
                        {
                            // handle paint selected word background and with partial word selection
                            var wordLine = DomUtils.GetCssLineBoxByWord(word);
                            var left = word.SelectedStartOffset > -1 ? word.SelectedStartOffset : ( wordLine.Words[0] != word && word.HasSpaceBefore ? -ActualWordSpacing : 0);
                            var width = word.SelectedEndOffset > -1 ? word.SelectedEndOffset + word.LastMeasureOffset.X / 2 : word.Width - word.LastMeasureOffset.X / 2 + (word.HasSpaceAfter ? ActualWordSpacing : 0);
                            g.FillRectangle(CssUtils.SelectionBackcolor, word.Left + offset.X + left, word.Top + offset.Y, width - left, wordLine.LineHeight);
                        }

                        g.DrawString(word.Text, font, brush, word.Left - word.LastMeasureOffset.X + offset.X, word.Top + word.LastMeasureOffset.Y + offset.Y);
                    }

                }

                for (int i = 0; i < rects.Length; i++)
                {
                    var actualRect = rects[i];
                    actualRect.Offset(offset);
                    PaintDecoration(g, actualRect, i == 0, i == rects.Length - 1);
                }

                // split paint to handle z-order
                foreach (CssBox b in Boxes)
                {
                    if(b.Position != CssConstants.Absolute)
                        b.Paint(g);
                }
                foreach (CssBox b in Boxes)
                {
                    if (b.Position == CssConstants.Absolute)
                        b.Paint(g);
                }

                RenderUtils.ReturnClip(g, prevClip);

                if (_listItemBox != null)
                {
                    _listItemBox.Paint(g);
                }
            }
        }

        /// <summary>
        /// Paints the border of the box
        /// </summary>
        /// <param name="g">the device to draw into</param>
        /// <param name="rectangle">the bounding rectangle to draw in</param>
        /// <param name="isFirst">is it the first rectangle of the element</param>
        /// <param name="isLast">is it the last rectangle of the element</param>
        protected void PaintBorder(Graphics g, RectangleF rectangle, bool isFirst, bool isLast)
        {
            if (rectangle.Width > 0 && rectangle.Height > 0)
            {
                //Top border
                if (!(string.IsNullOrEmpty(BorderTopStyle) || BorderTopStyle == CssConstants.None))
                {
                    var b = CssUtils.GetSolidBrush(BorderTopStyle == CssConstants.Inset ? DrawingUtils.Darken(ActualBorderTopColor) : ActualBorderTopColor);
                    DrawingUtils.DrawBorder(Border.Top, g, this, b, rectangle, isFirst, isLast);
                }

                //Right Border
                if (isLast && !(string.IsNullOrEmpty(BorderRightStyle) || BorderRightStyle == CssConstants.None))
                {
                    var b = CssUtils.GetSolidBrush(BorderRightStyle == CssConstants.Outset ? DrawingUtils.Darken(ActualBorderRightColor) : ActualBorderRightColor);
                    DrawingUtils.DrawBorder(Border.Right, g, this, b, rectangle, isFirst, true);
                }

                //Bottom border
                if (!(string.IsNullOrEmpty(BorderBottomStyle) || BorderBottomStyle == CssConstants.None))
                {
                    var b = CssUtils.GetSolidBrush(BorderBottomStyle == CssConstants.Outset ? DrawingUtils.Darken(ActualBorderBottomColor) : ActualBorderBottomColor);
                    DrawingUtils.DrawBorder(Border.Bottom, g, this, b, rectangle, isFirst, isLast);
                }

                //Left Border
                if (isFirst && !(string.IsNullOrEmpty(BorderLeftStyle) || BorderLeftStyle == CssConstants.None))
                {
                    var b = CssUtils.GetSolidBrush(BorderLeftStyle == CssConstants.Inset ? DrawingUtils.Darken(ActualBorderLeftColor) : ActualBorderLeftColor);
                    DrawingUtils.DrawBorder(Border.Left, g, this, b, rectangle, true, isLast);
                }
            }
        }

        /// <summary>
        /// Paints the background of the box
        /// </summary>
        /// <param name="g">the device to draw into</param>
        /// <param name="rectangle">the bounding rectangle to draw in</param>
        /// <param name="isFirst">is it the first rectangle of the element</param>
        /// <param name="isLast">is it the last rectangle of the element</param>
        protected void PaintBackground(Graphics g, RectangleF rectangle, bool isFirst, bool isLast)
        {
            //HACK: Background rectangles are being deactivated when justifying text.
            if (ContainingBlock.TextAlign != CssConstants.Justify && rectangle.Width > 0 && rectangle.Height > 0)
            {
                PaintBackgroundImage(g, rectangle, isFirst, isLast);

                Brush brush = null;
                bool dispose = false;
                SmoothingMode smooth = g.SmoothingMode;

                if (BackgroundGradient != CssConstants.None)
                {
                    brush = new LinearGradientBrush(rectangle, ActualBackgroundColor, ActualBackgroundGradient, ActualBackgroundGradientAngle);
                    dispose = true;
                }
                else if (CssUtils.IsColorVisible(ActualBackgroundColor))
                {
                    brush = CssUtils.GetSolidBrush(ActualBackgroundColor);
                }

                if(brush != null)
                {
                    // atodo: handle it correctly (tables background)
//                    if (isLast)
//                        rectangle.Width -= ActualWordSpacing + CssUtils.GetWordEndWhitespace(ActualFont);
                        
                    GraphicsPath roundrect = null;
                    if (IsRounded)
                    {
                        roundrect = DrawingUtils.GetRoundRect(rectangle, ActualCornerNW, ActualCornerNE, ActualCornerSE, ActualCornerSW);
                    }

                    if (HtmlContainer != null && !HtmlContainer.AvoidGeometryAntialias && IsRounded)
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }

                    if (roundrect != null)
                    {
                        g.FillPath(brush, roundrect);
                    }
                    else
                    {
                        g.FillRectangle(brush, rectangle);
                    }

                    g.SmoothingMode = smooth;

                    if (roundrect != null) roundrect.Dispose();
                    if(dispose) brush.Dispose();
                }
            }
        }

        /// <summary>
        /// Paint the background image of the box as specified by style.
        /// </summary>
        /// <param name="g">the device to draw into</param>
        /// <param name="rectangle">the bounding rectangle to draw in</param>
        /// <param name="isFirst">is it the first rectangle of the element</param>
        /// <param name="isLast">is it the last rectangle of the element</param>
        private void PaintBackgroundImage(Graphics g, RectangleF rectangle, bool isFirst, bool isLast)
        {
            if (_imageLoadHandler != null && _imageLoadHandler.Image != null && isFirst)
            {
                var srcRect = _imageLoadHandler.Rectangle == Rectangle.Empty
                                  ? new Rectangle(0, 0, Math.Min((int)rectangle.Width, _imageLoadHandler.Image.Width), Math.Min((int)rectangle.Height, _imageLoadHandler.Image.Height))
                                  : new Rectangle(_imageLoadHandler.Rectangle.Left, _imageLoadHandler.Rectangle.Top, Math.Min((int)rectangle.Width, _imageLoadHandler.Rectangle.Width), Math.Min((int)rectangle.Height, _imageLoadHandler.Rectangle.Height));

                var top = (int)rectangle.Top + (_imageLoadHandler.Rectangle == Rectangle.Empty ? 0 : ((int)rectangle.Height - srcRect.Height)/2 + 1);

                var destRect = new Rectangle((int)rectangle.Left + 2, top, Math.Min((int)rectangle.Width, srcRect.Width), Math.Min((int)rectangle.Height, srcRect.Height));

                g.DrawImage(_imageLoadHandler.Image, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// Paints the text decoration
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle"> </param>
        /// <param name="isFirst"> </param>
        /// <param name="isLast"> </param>
        protected void PaintDecoration(Graphics g, RectangleF rectangle, bool isFirst, bool isLast)
        {
            if (string.IsNullOrEmpty(TextDecoration) || TextDecoration == CssConstants.None) 
                return;

            float desc = CssUtils.GetDescent(ActualFont);
            float asc = CssUtils.GetAscent(ActualFont);
            float y = 0f;

            if (TextDecoration == CssConstants.Underline)
            {
                y = rectangle.Bottom - desc + 0.6f;
            }
            else if (TextDecoration == CssConstants.LineThrough)
            {
                y = rectangle.Bottom - desc - asc / 2;
            }
            else if (TextDecoration == CssConstants.Overline)
            {
                y = rectangle.Bottom - desc - asc - 2;
            }

            y -= ActualPaddingBottom - ActualBorderBottomWidth;

            float x1 = rectangle.X;
            float x2 = rectangle.Right;

            if (isFirst) x1 += ActualPaddingLeft + ActualBorderLeftWidth;
            if (isLast)
            {
                x2 -= ActualPaddingRight + ActualBorderRightWidth;
            }

            g.DrawLine(new Pen(ActualColor), x1, y, x2, y);
        }

        /// <summary>
        /// Offsets the rectangle of the specified linebox by the specified gap,
        /// and goes deep for rectangles of children in that linebox.
        /// </summary>
        /// <param name="lineBox"></param>
        /// <param name="gap"></param>
        internal void OffsetRectangle(CssLineBox lineBox, float gap)
        {
            if (Rectangles.ContainsKey(lineBox))
            {
                var r = Rectangles[lineBox];
                Rectangles[lineBox] = new RectangleF(r.X, r.Y + gap, r.Width, r.Height);
            }
        }

        /// <summary>
        /// Resets the <see cref="Rectangles"/> array
        /// </summary>
        internal void RectanglesReset()
        {
            _rectangles.Clear();
        }

        /// <summary>
        /// On image load process complete with image request refresh for it to be painted.
        /// </summary>
        /// <param name="image">the image loaded or null if failed</param>
        /// <param name="rectangle">the source rectangle to draw in the image (empty - draw everything)</param>
        /// <param name="async">is the callback was called async to load image call</param>
        private void OnImageLoadComplete(Image image, Rectangle rectangle, bool async)
        {
            if (image != null && async)
                HtmlContainer.RequestRefresh(false);
        }

        /// <summary>
        /// ToString override.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var tag = HtmlTag != null ? string.Format("<{0}>", HtmlTag.Name) : "anon";
            
            if (IsBlock)
            {
                return string.Format("{0}{1} Block {2}, Children:{3}", ParentBox == null ? "Root: " : string.Empty, tag, FontSize, Boxes.Count);
            }
            else if (Display == CssConstants.None)
            {
                return string.Format("{0}{1} None", ParentBox == null ? "Root: " : string.Empty, tag);
            }
            else
            {
                return string.Format("{0}{1} {2}: {3}", ParentBox == null ? "Root: " : string.Empty, tag, Display, Text);
            }
        }

        #endregion
    }
}