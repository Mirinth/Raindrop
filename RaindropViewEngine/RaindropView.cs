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
using System.IO;

public class RaindropView : IView
{
   public RaindropView(string viewPath, string masterPath)
   {
       this.ViewPath = viewPath;
   }

   public string ViewPath { get; private set; }

   public void Render(ViewContext viewContext, TextWriter writer)
   {
       string filePath = viewContext.HttpContext.Server.MapPath(this.ViewPath);

       Raindrop.Raindrop template = new Raindrop.Raindrop(filePath);

       template.Apply(viewContext.ViewData, writer);
   }
}