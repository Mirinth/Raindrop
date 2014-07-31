Copyright
=========
Copyright (C) 2014

Permission is granted to copy, distribute and/or modify this document
under the terms of the GNU Free Documentation License, Version 1.3
or any later version published by the Free Software Foundation;
with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.
A copy of the license is included in the file entitled "GNU
Free Documentation License" distributed with this document.

Syntax
======

Like HTML, Raindrop has tags and delimiters that open and close them. Each tag is delimited by a `<: ... :>` pair and has a name and one or more properties. Properties are of the type where being present means they're set, e.g. `<:tag-name property-value:>` rather than `<:tag-name property="value":>`. Raindrop treats the HTML inside and around its tags as plain text and doesn't assign any special meaning to anything outside a `<: ... :>` pair.

Raindrop tags
=============

Raindrop has four tags to handle its three use cases.

<:data name /:>
---------------

The `<:data:>` tag inserts a value into the output. When encountered, Raindrop looks up `name` in the IDictionary and writes that object to the output using its .ToString() method.

The space and / after `name` are both optional and ignored by the parser. They're allowed primarily for blending in with XML and XHTML surroundings.

<:cond 'name':> ... <:/cond 'comment':>
---------------------------------------

The `<:cond:>` tag conditionally processes a part of the template. Raindrop looks up `name` in its IDictionary, determines whether `name` is `true` and, if so, processes the contents of the `<:cond:>` block. Otherwise, the entire `<:cond:>` block is skipped.

The `<:cond:>` tag decides truthfulness in the following way:

1. If `name` is a boolean value, then the result is the boolean value of `name`.
2. If `name` is an IEnumerable with at least one element, then the result is true.
3. In all other cases, the result is false.

The closing `<:/cond:>` tag allows an additional comment between `cond` and `:>`. The comment is ignored by the parser so you can use it to write a reminder about which tag you're closing, e.g. `<:cond users-available:> ... <:/cond users-available:>`

<:ncond 'name':> ... <:/ncond 'comment':>
-----------------------------------------

The `<:ncond:>` tag functions identically to the `<:cond:>` tag, except that it only processes its contents if `name` is *false*. Truthfulness is decided in the exact same way as `<:cond:>`.

<:array 'name':> ... <:/array 'comment':>
-----------------------------------------

The `<:array:>` tag is processed once for each element in an array. It casts the `name` element in the IDictionary into an `IEnumerable<IDictionary<string, object>>` and processes its contents once for each IDictionary.

The `<:array:>` tag replaces the IDictionary for its contained block with one extracted from the IEnumerable, so contained blocks can't access names or values in the parent or sibling dictionaries.

`<:array:>` tags will result in their contents being processed zero times if `name` is an empty IEnumerable, effectively turning them into a false `<:cond:>` tag for empty IEnumerables.

`<:array:>` *does not* expect to get objects of an inappropriate type. If, for example, `name` happens to refer to a string object instead of an IEnumerable, then the templater will crash.

Like `<:/cond:>` and `<:/ncond:>`, the closing `<:/array:>` tag allows a comment to remind you what you're closing.