/*
 * Copyright 2014
 * 
 * This file is part of the Raindrop Templating Library.
 * 
 * The Raindrop Templating Library is free software: you can redistribute
 * it and/or modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 * 
 * The Raindrop Templating Library is distributed in the hope that it will
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with the Raindrop Templating Library. If not, see
 * <http://www.gnu.org/licenses/>. 
 */

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