Copyright
=========
Copyright (C) 2014

Permission is granted to copy, distribute and/or modify this document
under the terms of the GNU Free Documentation License, Version 1.3
or any later version published by the Free Software Foundation;
with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.
A copy of the license is included in the file entitled "GNU
Free Documentation License" distributed with this document.

Data passing
============

Inserting data into a controller's `ViewData` property is the typical way to pass data to a Raindrop view.

Raindrop expects a `System.IO.TextWriter` to write its output to, and an `IDictionary<string, object>` containing the data to output. Values in the `IDictionary` are all treated as `bool` (by conditional blocks), `System.Collections.IEnumerable` (by repeated blocks), and generic `object`s (by output blocks) depending on the context. The `.ToString()` method is called on an object to get a value to be written to the output.

`System.Web.Mvc.ViewDataDictionary` implements `IDictionary<string, object>`, so it can be used as the data dictionary. The MVC framework normally passes in a controller's `ViewData` property for this purpose, so your controller just needs to populate its `ViewData` property with the appropriate data.

The `TextWriter` is normally provided by the MVC framework and a controller doesn't normally have to worry about it at all.