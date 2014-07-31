Copyright
=========
Copyright (C) 2014

Permission is granted to copy, distribute and/or modify this document
under the terms of the GNU Free Documentation License, Version 1.3
or any later version published by the Free Software Foundation;
with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.
A copy of the license is included in the file entitled "GNU
Free Documentation License" distributed with this document.

Setup
=====

To set up Raindrop:

1. Open the solution file. Visual Studio C# 2010 Express was used to generate it; compatible programs may also work.
2. Build the Raindrop project.
3. If desired, move the .dll file in `/Raindrop/bin/Release/` to a more convenient location.
4. Open your ASP.Net MVC project.
5. Add an assembly reference to `Raindrop.dll` in `/Raindrop/bin/Release/` (or wherever you moved it to).
6. In the `/Global.asax.cs` file, add a new using statement: `using Raindrop;` at the top.
7. Inside the same file, find the method `MvcApplication.Application_Start()`.
8. Append the following line at the end of the method: `ViewEngines.Engines.Add(new RaindropViewEngine());`
9. (Optional) If you want to prevent any other view engines from running, you can put this line just before the one entered in (8): `ViewEngines.Engines.Clear();` There shouldn't be any significant difference between including or excluding the line unless you're also using another engine that thinks it's in charge of Raindrop templates (files ending in `.rdt`) (unlikely).

Adding template files
=====================

Raindrop expects the template file for a request to be at `/Views/{Controller}/{Action}.rdt`, just like most other view engines (except for the different extension, of course).