using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Core.SettingModule;
using Core.SDK.Db;
using Core.OracleModule;
using Core.OracleModule.Utils;
using System.Diagnostics;

namespace SettingModuleTest
{
    #region sql scripts
    /*
    create table test_setting(id number, username varchar2(256), section varchar2(256), subsection varchar2(256), setting_name varchar2(256), value varchar2(4000), clob_value CLOB, blob_value BLOB );
    create sequence test_setting_s;
     
    CREATE OR REPLACE package CHIPANDDALE.test_pkg as       
    procedure LoadSettings(p_username in varchar2, p_section in varchar2, p_subSection in varchar2, p_setiingCursor out sys_refcursor);
    procedure SaveSettings (p_username in varchar2, p_section in varchar2, p_subSection in varchar2, p_settingName in varchar2, p_settingvalue in varchar2, p_settingCLOBValue in CLOB, p_settingBLOBValue in BLOB);           
    end;
     
    CREATE OR REPLACE package body CHIPANDDALE.test_pkg as
        
        procedure LoadSettings(p_username in varchar2, p_section in varchar2, p_subSection in varchar2, p_setiingCursor out sys_refcursor) as
        begin
            open p_setiingCursor for
                select id, username, section, subsection, setting_name name, value, clob_value clobvalue, blob_value blobvalue
                from test_setting t
                where T.USERNAME =  p_username and
                          T.SECTION = p_section and
                          T.SUBSECTION = p_subSection;
        end;
    
        procedure SaveSettings (p_username in varchar2, p_section in varchar2, p_subSection in varchar2, p_settingName in varchar2, p_settingvalue in varchar2, p_settingCLOBValue in CLOB, p_settingBLOBValue in BLOB) as
        begin
            delete from  test_setting t
            where T.USERNAME =  p_username and
                      T.SECTION = p_section and
                      T.SUBSECTION = p_subSection and
                      T.SETTING_NAME = p_settingName;                
        
            insert into test_setting (id, username, section, subsection, setting_name, value, clob_value, blob_value)
            values (test_setting_s.Nextval, p_username, p_section, p_subSection, p_settingName, p_settingvalue, p_settingCLOBValue, p_settingBLOBValue);
        end;
    
    end;
    /
     
     */
    #endregion

    public enum TestEnum
    { 
        Zero = 0,
        First = 1,
        Other = 7,
        Unknown = 98765
    }
    
    [Flags]
    public enum TestEnumFlag
    {
        Zero = 0,
        First = 1,
        Second = 2,
        Forth = 4,
        Other = 7,
        Unknown = 98765
    }

    [TestClass]
    public class SettingModuleTest
    {
        public SettingModuleTest()
        {
            Environment.SetEnvironmentVariable("TNS_ADMIN", @"J:\Other_project\MyUtils\Deps\ODAC11\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.CL8MSWIN1251", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("Path", @"J:\Other_project\MyUtils\Deps\ODAC11\;" + Environment.GetEnvironmentVariable("Path"), EnvironmentVariableTarget.Process);
            _LogMgr = new Core.LogModule.LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
            _Logger = _LogMgr.GetLogger("SettingModuleTest");
            _Logger.Info("Start OracleModuleTest.");  
        }
        Core.SDK.Log.ILogMgr _LogMgr;
        Core.SDK.Log.ILogger _Logger;

        #region Setting classes
        internal class SettingWithSimpleTypeProperties
        {
            internal SettingWithSimpleTypeProperties()
            { }

            internal void Init()
            {
                StringProperty = "String!";
                Int64Property = 10;
                Int32Property = 20;
                Int16Property = 30;
                ByteProperty = 40;
                DateTimeProperty = new DateTime(2012, 1, 1, 12, 0, 0);
                SizeProperty = new Size(100, 100);
                PointProperty = new Point(200, 200);
                FloatProperty = 1.01f;
                DoubleProperty = 1.02d;
                DecimalProperty = 1.03M;
                ColorProperty = Color.Blue;
                GuidProperty = new Guid();
                TimeSpanProperty = new TimeSpan(123456);
                TestEnumProperty = TestEnum.Unknown;
                TestEnumFlagProperty = TestEnumFlag.First | TestEnumFlag.Second;
                FontProperty = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Pixel);
            }

            public override bool Equals(object obj)
            {
                SettingWithSimpleTypeProperties setting = obj as SettingWithSimpleTypeProperties;

                if (setting == null) return false;

                return (BoolProperty == setting.BoolProperty) &&
                       (StringProperty == setting.StringProperty) &&
                       (Int64Property == setting.Int64Property) &&
                       (Int32Property == setting.Int32Property) &&
                       (Int16Property == setting.Int16Property) &&
                       (ByteProperty == setting.ByteProperty) &&
                       (DateTimeProperty == setting.DateTimeProperty) &&
                       (SizeProperty == setting.SizeProperty) &&
                       (PointProperty == setting.PointProperty) &&
                       (FloatProperty == setting.FloatProperty) &&
                       (DoubleProperty == setting.DoubleProperty) &&
                       (DecimalProperty == setting.DecimalProperty) &&
                       (ColorProperty == setting.ColorProperty) &&
                       (GuidProperty == setting.GuidProperty) &&
                       (Font.Equals(FontProperty, setting.FontProperty)) &&
                       (TimeSpanProperty == setting.TimeSpanProperty) &&
                       (TestEnumProperty == setting.TestEnumProperty) &&
                       (TestEnumFlagProperty == setting.TestEnumFlagProperty);
            }

            public bool BoolProperty { get; set; }
            public string StringProperty { get; set; }
            public Int64 Int64Property { get; set; }
            public Int32 Int32Property { get; set; }
            public Int16 Int16Property { get; set; }
            public byte ByteProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public Size SizeProperty { get; set; }
            public Point PointProperty { get; set; }
            public float FloatProperty { get; set; }
            public double DoubleProperty { get; set; }
            public decimal DecimalProperty { get; set; }
            public Color ColorProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public Font FontProperty { get; set; }
            public TimeSpan TimeSpanProperty { get; set; } 
            public TestEnum TestEnumProperty { get; set; }
            public TestEnumFlag TestEnumFlagProperty { get; set; }            
        }

        internal class SettingWithNullableValueProperties 
        {
            internal SettingWithNullableValueProperties()
            { }

            internal void Init()
            {
                Int64NullableProperty = null;
                Int32NullableProperty = null;
                Int16NullableProperty = null;
                ByteNullableProperty = null;
                DateTimeNullableProperty = null;
                SizeNullableProperty = null;
                PointNullableProperty = null;
                FloatNullableProperty = null;
                DoubleNullableProperty = null;
                TimeSpanProperty = null;
                DecimalProperty = null;
                ColorProperty = null;
                TestEnumProperty = null;
                GuidProperty = null;
                FontProperty = null;
                TestEnumFlagProperty = null;
            }

            public override bool Equals(object obj)
            {
                SettingWithNullableValueProperties setting = obj as SettingWithNullableValueProperties;

                if (setting == null) return false;

                return (BoolNullableProperty == setting.BoolNullableProperty) &&
                       (Int64NullableProperty == setting.Int64NullableProperty) &&
                       (Int32NullableProperty == setting.Int32NullableProperty) &&
                       (Int16NullableProperty == setting.Int16NullableProperty) &&
                       (ByteNullableProperty == setting.ByteNullableProperty) &&
                       (DateTimeNullableProperty == setting.DateTimeNullableProperty) &&
                       (SizeNullableProperty == setting.SizeNullableProperty) &&
                       (PointNullableProperty == setting.PointNullableProperty) &&
                       (FloatNullableProperty == setting.FloatNullableProperty) &&
                       (DoubleNullableProperty == setting.DoubleNullableProperty) &&
                       (DecimalProperty == setting.DecimalProperty) &&
                       (ColorProperty == setting.ColorProperty) &&
                       (GuidProperty == setting.GuidProperty) &&
                       (Font.Equals(FontProperty, setting.FontProperty)) &&
                       (TimeSpanProperty == setting.TimeSpanProperty) &&
                       (TestEnumProperty == setting.TestEnumProperty) &&
                       (TestEnumFlagProperty == setting.TestEnumFlagProperty);
            }

            public bool? BoolNullableProperty { get; set; }
            public Int64? Int64NullableProperty { get; set; }
            public Int32? Int32NullableProperty { get; set; }
            public Int16? Int16NullableProperty { get; set; }
            public byte? ByteNullableProperty { get; set; }
            public DateTime? DateTimeNullableProperty { get; set; }
            public Size? SizeNullableProperty { get; set; }
            public Point? PointNullableProperty { get; set; }
            public float? FloatNullableProperty { get; set; }
            public double? DoubleNullableProperty { get; set; }
            public decimal? DecimalProperty { get; set; }
            public Color? ColorProperty { get; set; }
            public Guid? GuidProperty { get; set; }
            public Font FontProperty { get; set; }
            public TimeSpan? TimeSpanProperty { get; set; }
            public TestEnum? TestEnumProperty { get; set; }
            public TestEnumFlag? TestEnumFlagProperty { get; set; }    
        }

        internal class SettingBlobProperties
        {
            internal SettingBlobProperties()
            { }

            internal void Init()
            {
                Blob1Property = null; 
                Blob2Property = new byte[] { };
                Blob3Property = new byte[] { 1,2,3,4,5,6,7 };
            }

            public override bool Equals(object obj)
            {
                SettingBlobProperties setting = obj as SettingBlobProperties;

                if (setting == null) return false;

                return  CompareBlobs(Blob1Property, setting.Blob1Property) &&
                        CompareBlobs(Blob2Property, setting.Blob2Property) &&
                        CompareBlobs(Blob3Property, setting.Blob3Property);
            }

            public static bool CompareBlobs(byte[] b1, byte[] b2)
            {
                if ((b1 == null || b1.Length == 0) && (b2 == null || b2.Length == 0)) return true;

                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                        return false;                    
                }

                return true;
            }

            public byte[] Blob1Property { get; set; }
            public byte[] Blob2Property { get; set; }
            public byte[] Blob3Property { get; set; } 
        }

        internal class SettingListProperties
        {
            internal SettingListProperties()
            { }

            internal void Init()
            {
                List1Property = new List<string>();
                List1Property.Add("test1");
                List1Property.Add("test2");

                List2Property = new List<Int64>();
                List2Property.Add(10);
                List2Property.Add(20);

                List3Property = new List<Size>();
                List3Property.Add(new Size(10, 20));
                List3Property.Add(new Size(30, 40));

                List4Property = new List<string>();
                List4Property.Add("1");
                List4Property.Add("");
                List4Property.Add(null);
                List4Property.Add("2");

                List5Property = new List<Size?>();
                List5Property.Add(new Size(20, 23));
                List5Property.Add(null);
                List5Property.Add(new Nullable<Size>());

                List6Property = new List<TestEnumFlag?>();
                List6Property.Add(TestEnumFlag.Other | TestEnumFlag.Unknown);
                List6Property.Add(TestEnumFlag.Unknown);
                List6Property.Add(null);
                List6Property.Add(TestEnumFlag.Unknown);
            }

            public override bool Equals(object obj)
            {
                SettingListProperties setting = obj as SettingListProperties;

                if (setting == null) return false;

                return  CompareLists<string>(List1Property, setting.List1Property) &&
                        CompareLists<Int64>(List2Property, setting.List2Property) &&
                        CompareLists<Size>(List3Property, setting.List3Property) &&
                        CompareLists<string>(List4Property, setting.List4Property) &&
                        CompareLists<Size?>(List5Property, setting.List5Property);

            }

            static public bool CompareLists<T>(List<T> l1, List<T> l2)
            {
                if ((l1 == null || l1.Count == 0) && (l2 == null || l2.Count == 0)) return true;

                for (int i = 0; i < l1.Count; i++)
                {
                    if (typeof(T).Name == "String")
                    {
                        string str1 = l1[i] == null? "" : l1[i].ToString();
                        string str2 = l2[i] == null ? "" : l2[i].ToString();
                        return string.Equals(str1, str2);
                    }
                    if (typeof(T).Name == "Font")
                    {
                        return Font.Equals(l1[i], l2[i]);
                    }
                    if (Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        return Nullable.Equals(l1[i], l2[i]);
                    }

                    if ((l1[i] == null && l2[i] != null) ||
                        (l1[i] != null && l2[i] == null) ||
                        (!object.Equals(l1[i], l2[i])))
                        return false;                    
                }             

                return true;
            }

            public List<string> List1Property { get; set; }
            public List<Int64> List2Property { get; set; }
            public List<Size> List3Property { get; set; }
            public List<string> List4Property { get; set; }
            public List<Size?> List5Property { get; set; }
            public List<TestEnumFlag?> List6Property { get; set; }            
        }

        internal class SettingWithNullToEmptyAttributeProperties
        {
            internal SettingWithNullToEmptyAttributeProperties()
            { }

            internal virtual void Init()
            {
                String1Property = "str";
                PointProperty = new Point(11, 22);                
                StringListProperty = new List<string>() { "1", "2"};
                BlobProperty = new byte[] {1, 5};
                FontProperty = new Font("Arinal", 10);
                NColorProperty = new Color();
            }

            [Core.SDK.Setting.Attributes.NullToEmpty]
            public Font FontProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToEmpty]
            public string String1Property { get; set; }

            [Core.SDK.Setting.Attributes.NullToEmpty]
            public Point PointProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToEmpty]
            public Color? NColorProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToEmpty]
            public List<string> StringListProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToEmpty]
            public byte[] BlobProperty { get; set; }           
        }

        internal class SettingWithIgnoreAttributeProperties
        {
            internal SettingWithIgnoreAttributeProperties()
            { }

            internal virtual void Init()
            {
                String1Property = "str";
                PointProperty = new Point(11, 22);
                StringListProperty = new List<string>() { "1", "2" };
            }

            [Core.SDK.Setting.Attributes.Ignore]
            public string String1Property { get; set; }

            [Core.SDK.Setting.Attributes.Ignore]
            public Point PointProperty { get; set; }

            [Core.SDK.Setting.Attributes.Ignore]
            public List<string> StringListProperty { get; set; }
        }

        internal class SettingWithNullNotChangedAttributeProperties
        {
            internal SettingWithNullNotChangedAttributeProperties()
            { }

            internal virtual void Init()
            {
                String1Property = "str";
                PointProperty = new Point(11, 22);
                StringListProperty = new List<string>() { "1", "2" };
            }

            [Core.SDK.Setting.Attributes.NullNotChanged]
            public string String1Property { get; set; }

            [Core.SDK.Setting.Attributes.NullNotChanged]
            public Point PointProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullNotChanged]
            public List<string> StringListProperty { get; set; }
        }

        internal class SettingWithNullToDefaultAttributeProperties
        {
            internal SettingWithNullToDefaultAttributeProperties()
            { }

            internal virtual void Init()
            {
                String1Property = "str";
                PointProperty = new Point(11, 22);
                StringListProperty = new List<string>() { "1", "2" };
            }

            [Core.SDK.Setting.Attributes.NullToDefault("False")]
            public bool BoolProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("default")]
            public string String1Property { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("55, 77")]
            public Point PointProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("88, 99")]
            public Size SizeProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("00:00:00.0123456")]
            public TimeSpan TimeSpanProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("11.11.1111 11:11:11")]
            public DateTime DateTimeProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("11.11.1111 11:11:11")]
            public DateTime? DateTime2Property { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("")]
            public DateTime? DateTime3Property { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault(new byte[] {1,2,3,4})]
            public byte[] BlobProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("1#@#2#@#3#@#4")]
            public List<string> StringListProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("Arial, 10px, style=Bold")]
            public Font FontProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("Arial, 10px, style=Bold#@#Arial, 10px, style=Bold#@#Arial, 10px, style=Bold")]
            public List<Font> FontListProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("Unknown")]
            public TestEnum TestEnumProperty { get; set; }

            [Core.SDK.Setting.Attributes.NullToDefault("First, Second")]
            public TestEnumFlag TestEnumFlagProperty { get; set; }
        }
        #endregion Setting calsses




        [TestMethod]
        public void TestSaveLoadSimpleTypeProperty()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingWithSimpleTypeProperties setting = new SettingWithSimpleTypeProperties();
                setting.Init();
                SettingWithSimpleTypeProperties setting2 = new SettingWithSimpleTypeProperties();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);                
                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting2);

                Debug.Assert(setting.Equals(setting2), "Saved and loaded settings are not equal  (value types).");
            }
        }

        [TestMethod]
        public void TestSaveLoadNullableValueProperty()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingWithNullableValueProperties setting = new SettingWithNullableValueProperties();
                setting.Init();
                SettingWithNullableValueProperties setting2 = new SettingWithNullableValueProperties();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);
                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting2);

                Debug.Assert(setting.Equals(setting2), "2. Saved and loaded settings are not equal (nullable types).");
            }
        }

        [TestMethod]
        public void TestSaveLoadBlobProperty()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingBlobProperties setting = new SettingBlobProperties();
                setting.Init();
                SettingBlobProperties setting2 = new SettingBlobProperties();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);
                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting2);

                Debug.Assert(setting.Equals(setting2), "2. Saved and loaded settings are not equal (blob types).");
            }
        }

        [TestMethod]
        public void TestSaveLoadListProperty()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingListProperties setting = new SettingListProperties();
                setting.Init();
                SettingListProperties setting2 = new SettingListProperties();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);
                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting2);

                Debug.Assert(setting.Equals(setting2), "2. Saved and loaded settings are not equal (List types).");
            }
        }

        [TestMethod]
        public void TestNullToEmptyAttribute()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);                

                SettingWithNullToEmptyAttributeProperties setting = new SettingWithNullToEmptyAttributeProperties();
                setting.Init();                

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);

                OraCommand command = new OraCommand("delete from test_setting");
                command.CommandType = System.Data.CommandType.Text;
                using (DbTransaction tr = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    tr.Success = true;
                }

                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting);

                Debug.Assert(setting.String1Property != null, "Null string property");
                Debug.Assert(setting.PointProperty == Point.Empty, "Not empty Point property");                
                Debug.Assert(setting.StringListProperty != null, "String list property is null");
                Debug.Assert(setting.BlobProperty != null, "Blob list property is null");
                Debug.Assert(setting.FontProperty != null, "Font list property is null");                
            }
        }

        [TestMethod]
        public void TesIgnoreAttribute()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingWithIgnoreAttributeProperties setting = new SettingWithIgnoreAttributeProperties();
                setting.Init();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);
                setting.PointProperty = new Point(33,44);
                setting.String1Property = "after";
                setting.StringListProperty = new List<string>() {"22", "33" };
                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting);

                Debug.Assert(setting.String1Property == "after", "String1Property not ignored");
                Debug.Assert(setting.PointProperty == new Point(33, 44), "PointProperty not ignored");
                Debug.Assert(SettingListProperties.CompareLists<string>(setting.StringListProperty, new List<string>() { "22", "33" }), "StringListProperty not ignored");
            }
        }

        [TestMethod]
        public void TestNullNotChanged()
        {
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingWithNullNotChangedAttributeProperties setting = new SettingWithNullNotChangedAttributeProperties();
                setting.Init();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);

                OraCommand command = new OraCommand("delete from test_setting");
                command.CommandType = System.Data.CommandType.Text;
                using (DbTransaction tr = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    tr.Success = true;
                }

                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting);

                Debug.Assert(setting.String1Property == "str", "String1Property is changed");
                Debug.Assert(setting.PointProperty == new Point(11, 22), "PointProperty is changed");
                Debug.Assert(SettingListProperties.CompareLists<string>(setting.StringListProperty, new List<string>() { "1", "2" }), "StringListProperty is changed");
            }
        }

        [TestMethod]
        public void TestNullToDefault()
        {            
            SettingMgr settingMgr = new SettingMgr(_LogMgr);
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(dbManager, _LogMgr);

                SettingWithNullToDefaultAttributeProperties setting = new SettingWithNullToDefaultAttributeProperties();
                setting.Init();

                settingMgr.SaveSettings("User2", "Section1", "Subsection1", setting);

                OraCommand command = new OraCommand("delete from test_setting");
                command.CommandType = System.Data.CommandType.Text;
                using (DbTransaction tr = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    tr.Success = true;
                }

                settingMgr.LoadSetting("User2", "Section1", "Subsection1", setting);

                Debug.Assert(setting.BoolProperty == false, "BoolProperty not equal to default");
                Debug.Assert(setting.String1Property == "default", "String1Property not equal to default");
                Debug.Assert(setting.PointProperty == new Point(55, 77), "PointProperty not equal to default");
                Debug.Assert(setting.SizeProperty == new Size(88, 99), "SizeProperty not equal to default");
                Debug.Assert(setting.DateTimeProperty == new DateTime(1111, 11, 11, 11, 11, 11), "DateTime not equal to default");
                Debug.Assert(setting.DateTime2Property == new DateTime(1111, 11, 11, 11, 11, 11), "DateTime2 not equal to default");
                Debug.Assert(setting.DateTime3Property == null, "DateTime3 not equal to default");
                Debug.Assert(SettingBlobProperties.CompareBlobs(setting.BlobProperty, new byte[] { 1, 2, 3, 4 }), "BlobProperty not equal to default");
                Debug.Assert(SettingListProperties.CompareLists<string>(setting.StringListProperty, new List<string>() { "1", "2", "3", "4" }), "StringListProperty not equal to default");
                /*Debug.Assert(SettingListProperties.CompareLists<Font>(setting.FontListProperty, new List<Font>() {new Font("Arial", 10, FontStyle.Bold), 
                                                                                                                  new Font("Arial", 10, FontStyle.Bold),
                                                                                                                  new Font("Arial", 10, FontStyle.Bold)}), "FontListProperty not equal to default");
                Debug.Assert(Font.Equals(setting.FontProperty, new Font("Arial", 10, FontStyle.Bold)), "FontProperty not equal to default");*/
                Debug.Assert(setting.TestEnumProperty == TestEnum.Unknown, "TestEnumProperty not equal to default");
                Debug.Assert(setting.TestEnumFlagProperty == (TestEnumFlag.First | TestEnumFlag.Second), "TestEnumFlagProperty not equal to default");
            }
        }
    }

    
}
