<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<h1>Demonstration (for Internet Explorer)</h1>
<form id="form1" runat="server">
<p>
This page is a demonstration of the control, running in a limited trust environment.
</p>
<p>
This page works for Internet Explorer (IE) only.
IE can host .NET user controls, embedded in the HTML as an <code>&lt;object&gt;</code> element.
The requirements are:
</p>
<ul class="List">
<li>Internet Explorer</li>
<li>.NET framework (version 2 or later) installed on your machine, 
and JavaScript enabled (so that the page can interact with the control).</li>
<li>For IE8: this web site added to your list of 'Trusted sites', or 'Local intranet'.</li>
</ul>
<p>
Until recently, this demonstration would run in the 'Internet' security zone as well.
However, Microsoft has changed Internet Explorer's ability to run .NET user controls, as explained in
<a rel="nofollow" target="_blank" href="http://blogs.msdn.com/b/ieinternals/archive/2009/10/09/dotnet-usercontrols-do-not-load-in-ie8-internet-zone.aspx">DotNet UserControls Restricted in IE8</a>.
</p>
<div>

<object id="MyEditControl"    
    classid="http:ModelEditControl.dll#ModelText.WebEditor"    
    height="300"
    width="100%">
</object>

<asp:HiddenField ID="HiddenField" runat="server" />

<p>
    <asp:Button
        ID="SubmitButton"
        OnClientClick="beforeSubmit()"
        runat="server"
        Text="Submit" onclick="Submit_Click" />
</p>
    <asp:Panel ID="PanelOfResults" runat="server" Visible="false">
 <p>
       You submitted the following text:
 </p>
       <pre>
        <asp:Label ID="LabelOfResults" runat="server" Text="Label"></asp:Label>
        </pre>
    </asp:Panel>

</div>
</form>
</body>
</html>
