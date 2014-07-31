Copyright
=========
Copyright (C) 2014

Permission is granted to copy, distribute and/or modify this document
under the terms of the GNU Free Documentation License, Version 1.3
or any later version published by the Free Software Foundation;
with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.
A copy of the license is included in the file entitled "GNU
Free Documentation License" distributed with this document.

Raindrop
========

Raindrop is a templating library for generating HTML documents. Like the HTML it generates, it's a declarative, tag-based language. It also functions as a kind of meta-HTML, surrounding regular HTML with special tags that give the HTML more meaning.

Features
========

Raindrop supports three of the common needs that get servers involved in processing a document:
- Inserting dynamically generated data into a static template
- Conditionally including blocks of HTML
- Repeating blocks of HTML for each element of an array

Embedding other templates isn't supported yet.

Environment
===========

Raindrop is written in C# and implements the ASP.Net MVC framework's IView and IViewEngine interfaces, making it ready for use in ASP.Net projects built on the MVC framework. It can also be used in other projects, but they'll have to add a reference to System.Web.Mvc to get access to the types Raindrop expects to use.