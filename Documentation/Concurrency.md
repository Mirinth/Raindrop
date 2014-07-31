Copyright
=========
Copyright (C) 2014

Permission is granted to copy, distribute and/or modify this document
under the terms of the GNU Free Documentation License, Version 1.3
or any later version published by the Free Software Foundation;
with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.
A copy of the license is included in the file entitled "GNU
Free Documentation License" distributed with this document.

Concurrency & Reuse
===================

Raindrop is thread-safe as long as its template files aren't modified while loading, its `IDictionary`s aren't modified while being used, and its `TextWriters` aren't shared among multiple threads.

A fully constructed template object is no longer dependent on the template file, so the template file can be safely modified while Raindrop isn't constructing objects from it. Raindrop constructs a new template object every time a `View` is applied though, so the file is often in use.

Templates are loaded with FileAccess.Read and FileShare.Read permissions, so Raindrop can safely share access with other readers (but not other writers). Raindrop can also safely share templates with itself (important for multithreading).

A fully constructed template object is imutable, so it can be cached and reused repeatedly. The template object can also be used by many threads simultaneously without issue. However, Raindrop reads from its IDictionary and writes to its TextWriter, so writing to the IDictionary or using the TextWriter at all from another thread may result in race conditions.

Common use
==========

The common use case is to let the MVC framework fetch your controller's `ViewData` property after the controller runs, then feed that into the Raindrop view engine with a `TextWriter` made specifically for the current request.

No special considerations need to be made for this use. The MVC framework plays very nicely with Raindrop.

Editing templates
=================

If you edit a template (or access it from another program) while Raindrop is running, then Raindrop may crash with an `UnauthorizedAccessException` because it can't obtain FileShare.Read and FileAccess.Read permissions. To update a template without the crash, Raindrop needs to be stopped first.