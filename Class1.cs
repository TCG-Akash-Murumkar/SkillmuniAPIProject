using System;

public class Requestor
{
    public string no { get; set; }
    public string name { get; set; }
    public string adid { get; set; }
}

public class Workflow
{
    public string no { get; set; }
    public string name { get; set; }
    public string adid { get; set; }
    public string level { get; set; }
    public string leveltext { get; set; }
}

public class Approver
{
    public string leveltext { get; set; }
}

public class Displaydetails
{
    public string Summary { get; set; }
    public string Duration { get; set; }
    public string CreatedDate { get; set; }
    public string ismodified { get; set; }
    public string autoapprovaldate { get; set; }
    public string Period { get; set; }
    public List<object> fields { get; set; }
}

public class Postbackparams
{
}

public class Action
{
    public string applicable { get; set; }
    public string approveurl { get; set; }
    public string rejecturl { get; set; }
    public string reverturl { get; set; }
    public string appurl { get; set; }
    public Postbackparams postbackparams { get; set; }
}

public class Buttons
{
    public string canapprove { get; set; }
    public string canreject { get; set; }
    public string canseekclarification { get; set; }
    public string apprcaption { get; set; }
    public string rejcaption { get; set; }
    public string seekcaption { get; set; }
}

public class Layout
{
    public string type { get; set; }
    public string mwurl { get; set; }
    public string ssourl { get; set; }
    public Action action { get; set; }
    public Buttons buttons { get; set; }
    public List<object> templateinfo { get; set; }
}

public class MT
{
    public string appname { get; set; }
    public string title { get; set; }
    public string transstatus { get; set; }
    public string lastmodifieddate { get; set; }
    public string notification { get; set; }
    public Approver approver { get; set; }
    public Displaydetails displaydetails { get; set; }
    public List<Layout> layout { get; set; }
}

public class Approver2
{
    public string leveltext { get; set; }
}

public class Displaydetails2
{
    public string Summary { get; set; }
    public string Duration { get; set; }
    public string CreatedDate { get; set; }
    public string ismodified { get; set; }
    public string autoapprovaldate { get; set; }
    public string Period { get; set; }
    public List<object> fields { get; set; }
}

public class Postbackparams2
{
}

public class Action2
{
    public string applicable { get; set; }
    public string approveurl { get; set; }
    public string rejecturl { get; set; }
    public string reverturl { get; set; }
    public string appurl { get; set; }
    public Postbackparams2 postbackparams { get; set; }
}

public class Buttons2
{
    public string canapprove { get; set; }
    public string canreject { get; set; }
    public string canseekclarification { get; set; }
    public string apprcaption { get; set; }
    public string rejcaption { get; set; }
    public string seekcaption { get; set; }
}

public class Layout2
{
    public string type { get; set; }
    public string mwurl { get; set; }
    public string ssourl { get; set; }
    public Action2 action { get; set; }
    public Buttons2 buttons { get; set; }
    public List<object> templateinfo { get; set; }
}

public class YR
{
    public string appname { get; set; }
    public string title { get; set; }
    public string transstatus { get; set; }
    public string lastmodifieddate { get; set; }
    public string notification { get; set; }
    public Approver2 approver { get; set; }
    public Displaydetails2 displaydetails { get; set; }
    public List<Layout2> layout { get; set; }
}

public class Itemlist
{
    public string appid { get; set; }
    public string transid { get; set; }
    public string overalltransstatus { get; set; }
    public string translastmodifieddate { get; set; }
    public Requestor requestor { get; set; }
    public List<Workflow> workflow { get; set; }
    public List<object> PA { get; set; }
    public List<object> AA { get; set; }
    public List<MT> MT { get; set; }
    public List<YR> YR { get; set; }
}

public class RootObject
{
    public Itemlist itemlist { get; set; }
}


