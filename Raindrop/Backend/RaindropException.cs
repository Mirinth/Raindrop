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

using System;
using System.Text;

namespace Raindrop.Backend
{
    enum ErrorCode
    {
        TagStreamEmpty,
        TagStreamAtTag,
        TagStreamAtText,
        TemplateFormat,
        TagNotSupported,
        ParameterMissing,
        AppliedEOF,
        EndTagMismatch,
        MissingKey,
    }

    class RaindropException : Exception
    {
        public ErrorCode Code { get; set; }
        public int Index { get; set; }
        public string FilePath { get; set; }

        public RaindropException(
            string message,
            string filePath,
            int templateIndex,
            ErrorCode code)
            : base(message)
        {
            Code = code;
            Index = templateIndex;
            FilePath = filePath;
        }
    }
}