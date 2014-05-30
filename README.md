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

Inputs
======

Raindrop expects a System.IO.TextWriter to write its output to, and a System.Web.Mvc.ViewDataDictionary containing the data to output. Values in the ViewDataDictionary are all treated as bool (by conditional blocks), System.Collections.IEnumerable (by repeated blocks), and generic objects (by output blocks) depending on the context. The .ToString() method is called on an object to get a value to be written to the output.

Syntax
======

Like HTML, Raindrop has tags and delimiters that open and close them. Each tag is delimited by a <: and :> pair and has a name and one or more properties. Properties are of the type where being present means they're set, e.g. <:tag-name property-value:> rather than <:tag-name property="value":>. Raindrop treats the HTML inside and around its tags as plain text and doesn't assign any special meaning to anything outside a <: and :> pair.

Raindrop tags
=============

Raindrop has four tags to handle its three use cases.

<:data name /:>
---------------

The <:data:> tag inserts a value into the output. When encountered, Raindrop looks up 'name' in the ViewDataDictionary and writes that object to the output using its .ToString() method.

The space and / after 'name' are both optional and ignored by the parser. They're allowed primarily for blending in with XML and XHTML surroundings.

<:cond 'name':> ... <:/cond 'comment':>
---------------------------------------

The <:cond:> tag conditionally processes a part of the template. Raindrop looks up 'name' in its ViewDataDictionary, determines whether 'name' is 'true' and, if so, processes the contents of the <:cond:> block. Otherwise, the entire <:cond:> block is skipped.

The <:cond:> tag decides truthfulness in the following way:

1. If 'name' isn't in the ViewDataDictionary, then the result is false.
2. If 'name' is a boolean value, then the result is the boolean value of 'name'.
3. If 'name' is an IEnumerable with at least one element, then the result is true.
4. In all other cases, the result is false.

The closing <:/cond:> tag allows an additional comment between 'cond' and ':>'. The comment is ignored by the parser so you can use it to write a reminder about which tag you're closing, e.g. <:cond users-available:> ... <:/cond users-available:>

<:ncond 'name':> ... <:/ncond 'comment':>
-----------------------------------------

The <:ncond:> tag functions identically to the <:cond:> tag, except that it only processes its contents if 'name' is *false*. Truthfulness is decided in the exact same way as <:cond:>.

<:array 'name':> ... <:/array 'comment':>
-----------------------------------------

The <:array:> tag is processed once for each element in an array. It casts the 'name' element in the ViewDataDictionary into an IEnumerable<ViewDataDictionary> and processes its contents once for each ViewDataDictionary.

The <:array:> tag replaces the ViewDataDictionary for its contained block with one extracted from the IEnumerable, so contained blocks can't access names or values in the parent or sibling dictionaries.

<:array:> tags will result in their contents being processed zero times if 'name' is an empty IEnumerable, effectively turning them into a false <:cond:> tag for empty IEnumerables.

Unlike <:cond:> and <:ncond:>, <:array:> *does not* expect to get objects of an inappropriate type. If, for example, 'name' happens to refer to a string object instead of an IEnumerable<ViewDataDictionary>, then the templater will crash.

Like <:/cond:> and <:/ncond:>, the closing <:/array:> tag allows a comment to remind you what you're closing.

Example
=======

Template
--------

```
<html>
	<head>
		<title><:data page-title /:></title>
	</head>
	<body>
		<h1><:data page-head /:></h1>
<:ncond posts:>
  No comments are available.
<:/ncond posts:>
<:cond posts:>
		<table>
<:array posts:>
			<tr>
				<td bgcolor='blue'><:data date /:></td>
				<td bgcolor='blue'><:data name /:></td>
			</tr>
			<tr>
				<td colspan='2' bgcolor='pink'><:data content /:></td>
			</tr>
<:/array posts:>
		</table>
<:/cond posts:>
	</body>
</html>
```

Dictionary
----------

```C#
ViewDataDictionary data = {
	string page-title = "Recent posts - example.com"
	string page-head = "Recent posts"
	IEnumerable<ViewDataDictionary> posts = {
		1 = {
			string name = "Alice"
			DateTime date = 12/24/2014 09:17
			string content = "Hello, everyone!"
		}
		2 = {
			string name = "Bob"
			DateTime date = 12/24/2014 09:19
			string content = "Hi!"
		}
		3 = {
			string name = "Cindy"	
			DateTime date = 12/24/2014 09:20
			string content = "Hey, how's it  going?"
		}
	}
}
```

Explanation
-----------

The template inserts the data named 'page-title' inside the HTML `<title>` element and 'page-head' in the `<h1>` element. It checks if 'posts' is *not* true and prints a message telling the user there were no posts if so. Next, it checks if 'posts' *is* true and prints them out if so. Printing involves outputting a `<table>` element, followed by two `<tr>`s and three `<td>`s for each element in an array and completed by closing the `<table>` tag. Finally, it closes the `<body>` and `<html>` tags.

Result
------

```HTML
<html>
	<head>
		<title>Recent posts - example.com</title>
	</head>
	<body>
		<h1>Recent posts</h1>

		<table>

			<tr>
				<td bgcolor='blue'>12/24/2014 09:17</td>
				<td bgcolor='blue'>Alice</td>
			</tr>
			<tr>
				<td colspan='2' bgcolor='pink'>Hello, everyone!</td>
			</tr>

			<tr>
				<td bgcolor='blue'>12/24/2014 09:19</td>
				<td bgcolor='blue'>Bob</td>
			</tr>
			<tr>
				<td colspan='2' bgcolor='pink'>Hi!</td>
			</tr>

			<tr>
				<td bgcolor='blue'>12/24/2014 09:20</td>
				<td bgcolor='blue'>Cindy</td>
			</tr>
			<tr>
				<td colspan='2' bgcolor='pink'>Hey, how's it going?</td>
			</tr>

		</table>

	</body>
</html>
```

Concurrency & Reuse
===================

Raindrop is thread-safe as long as its template files aren't modified while loading, its ViewDataDictionaries aren't modified while being used, and its TextWriters aren't shared among multiple threads.

A fully constructed template object is no longer dependent on the template file, so the template file can be safely modified while Raindrop isn't constructing objects from it. Raindrop currently constructs a template object for every page load, which means that updates to the template are noticed immediately, but also means that updates can cause Raindrop to crash or behave erratically. Templates are loaded using System.IO.File.ReadAllText(), which may or may not allow other programs to modify a template while Raindrop is using it.

A fully constructed template object is imutable, so it can be cached and reused repeatedly and by many threads. However, Raindrop reads from its ViewDataDictionary and writes to its TextWriter, so writing to the ViewDataDictionary or using the TextWriter at all from another thread may result in race conditions.

Setup
=====

To set up Raindrop:

1. Open the solution file. Visual Studio C# 2010 Express was used to generate it; compatible programs may also work.
2. Build both projects.
3. If desired, move the two .dll files in `/RaindropViewEngine/bin/Release/` to a more convenient location.
4. Open your ASP.Net MVC project.
5. Add an assembly reference to `RaindropViewEngine.dll` in `/RaindropViewEngine/bin/Release/` (or wherever you moved it to).
6. In the `/Global.asax.cs` file, add a new using statement: `using Raindrop;` at the top.
7. Inside the same file, find the method `MvcApplication.Application_Start()`.
8. Append the following line at the end of the method: `ViewEngines.Engines.Add(new RaindropViewEngine());`
9. (Optional) If you want to prevent any other view engines from running, you can put this line just before the one entered in (8): `ViewEngines.Engines.Clear();` There shouldn't be any significant difference between including or excluding the line unless you're also using another engine that thinks it's in charge of Raindrop templates (files ending in `.rdt`) (unlikely).

Adding template files
=====================

Raindrop expects the template file for a request to be at `/Views/{Controller}/{Action}.rdt`, just like most other view engines (except for the extension, of course).

Still working on...
===================

While Raindrop is useful as-is, it still has a few rough edges to be smoothed out.
- No escape sequence for including <: and :> directly in templates
- The exception system doesn't cleanly seperate parsing exceptions from templating ones
- Error reporting in general is a little messy
- No support for embedded templates
- Unit tests (and possibly code contracts) would be worthwhile
- A template testing system would be helpful
- Unknown effects when another program tries to modify a template in use by Raindrop
- Templates aren't cached, even though they could benefit from it (I just haven't learned how to cache them yet)
- Raindrop needs to be recompiled to add new tags, even though it isn't absolutely necessary (I'm just not familiar with reflection yet).
- The parser leaves blank/whitespace-filled lines if a Raindrop tag was the only thing there. Fixing this is going to be complicated because some tags (e.g. <:data:>) need the whitespace while others (e.g. <:cond:>) don't, so the parser will need knowledge of tags, which means a lot of added complexity.