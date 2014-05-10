using System.Web.Mvc;

public class RaindropViewEngine : VirtualPathProviderViewEngine
{
    public RaindropViewEngine()
    {
        // This is where we tell MVC where to look for our files. This says
        // to look for a file at "Views/Controller/Action.html"
        base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.rdl" };

        base.PartialViewLocationFormats = base.ViewLocationFormats;
    }

    protected override IView CreateView(ControllerContext context, string viewPath, string masterPath)
    {
        return new RaindropView(viewPath, masterPath);
    }

    protected override IView CreatePartialView(ControllerContext context, string partialPath)
    {
        return new RaindropView(partialPath, "");
    }
}