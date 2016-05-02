using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HiddenField.Value =
@"
 <body>
  <h1>Sample programs</h1>
  <h2>Support</h2>
  <p>Please post a message on the <a href=""http://groups.google.com/group/modeltext"">ModelText discussion group</a> to say whether you like this software, to make suggestions, to ask questions, and any/or for bug reports.</p>
  <h2>How to</h2>
  <p>Here are some notes about how I wrote the sample programs (for example, the FileOpenAndSave project):</p>
  <ul>
   <li>Use Visual Studio to create a new, empty Windows Forms Application.</li>
   <li>Use the Visual Studio Designer, to design the layout of the form: the form for this example project simply includes one application menu strip, plus a panel on which the ModelText HTML control is superimposed at run-time. Also use the Visual Studio Designer to define the menu items, and to declare the events handlers which implement each menu item.</li>
   <li>Read <a href=""http://www.modeltext.com/html/apis/Index.html"">APIs for the HTML Control</a> for a description of how to use the ModelText HTML control, and add the corresponding statements to the Form1.cs source file.</li>
  </ul>
  <h2>List of samples</h2>
  <p>There are two sample programs:</p>
  <table>
   <tr>
    <td>FileOpenAndSave</td>
    <td>This program shows how to include the following functionality: 
     <ul>
      <li>Load and save a document</li>
      <li>Implement the 'Save' and 'Insert Hyperlink' commands</li>
     </ul>
    </td>
   </tr>
   <tr>
    <td>FormControls</td>
    <td>This program shows how to display a form, and get the contents of the form controls when the user clicks the submit button.</td>
   </tr>
  </table>
  <p></p>
 </body>
";
        }

        string initializeEditControl = string.Format(
@"
function getHiddenControl() {{
    return document.getElementById('{0}');
}}
function getSubmitControl() {{
    return document.getElementById('{1}');
}}
",
            HiddenField.ClientID,
            SubmitButton.ClientID
            );

        initializeEditControl +=

@"
function getEditControl() {
    return document.getElementById('MyEditControl');
}
function startPolling() {
    setTimeout(pollForEvents, 1000);
}
function pollForEvents() {
    var editControl = getEditControl();
    var haveSaveRequest = editControl.haveSaveRequest();
    if (haveSaveRequest)
    {
        var submitControl = getSubmitControl();
        submitControl.click();
    }
    startPolling();
}
function beforeSubmit()
{
    var editControl = getEditControl();
    var hiddenControl = getHiddenControl();
    var document = editControl.getDocumentFragment();
    hiddenControl.value = document;
}
function initializeEditControl() {
    var editControl = getEditControl();
    var hiddenControl = getHiddenControl();
    editControl.text = hiddenControl.value;
    editControl.setDocumentFragment(hiddenControl.value);
    startPolling();
}
initializeEditControl();
";

        ClientScript.RegisterStartupScript(
            GetType(),
            "initializeEditControl",
            initializeEditControl,
            true);
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        string document = Server.HtmlEncode(HiddenField.Value);
        PanelOfResults.Visible = true;
        LabelOfResults.Text = document;
    }
}
