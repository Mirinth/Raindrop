using System.Web.Mvc;

public class HoTMeaTViewEngine : VirtualPathProviderViewEngine
{
    public HoTMeaTViewEngine()
    {
        // This is where we tell MVC where to look for our files. This says
        // to look for a file at "Views/Controller/Action.html"
        base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.html" };

        base.PartialViewLocationFormats = base.ViewLocationFormats;
    }

    protected override IView CreateView(ControllerContext context, string viewPath, string masterPath)
    {
        return new HoTMeaTView(viewPath, masterPath);
    }

    protected override IView CreatePartialView(ControllerContext context, string partialPath)
    {
        return new HoTMeaTView(partialPath, "");
    }
}