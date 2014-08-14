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

/*
 * The IView class for Raindrop, used by the MVC framework.
 * This wraps the Raindrop class to provide the IView
 * interface for MVC.
 */

using System;
using System.IO;
using System.Web.Mvc;

public class RaindropView : IView
{
    public RaindropView(string viewPath, string masterPath)
    {
        this.ViewPath = viewPath;
    }

    public string ViewPath { get; private set; }

    public void Render(ViewContext viewContext, TextWriter writer)
    {
        Raindrop.Raindrop template;
        Func<string, TextReader> mapper = (unmappedPath) =>
        {
                return Map(viewContext.HttpContext.Server.MapPath, unmappedPath);
        };

        template = new Raindrop.Raindrop(this.ViewPath, mapper);

        template.Apply(writer, viewContext.ViewData);
    }

    /// <summary>
    /// Maps an unmapped path using a mapper; opens it with FileMode.Open,
    /// FileAccess.Read and FileShare.Read settings; wraps it in a TextReader
    /// and returns the reader
    /// </summary>
    /// <param name="mapper">
    /// The method to be used to map the path.
    /// </param>
    /// <param name="unmappedPath">
    /// The unmapped path to map and open.
    /// </param>
    /// <returns>A TextReader with the contents of the mapped path.</returns>
    private TextReader Map(Func<string, string> mapper, string unmappedPath)
    {
        string mappedPath = mapper(unmappedPath);
        FileStream templateFile = new FileStream(
            mappedPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read);
        StreamReader templateReader = new StreamReader(templateFile);
        return templateReader;
    }
}