Copyright
=========
Copyright (C) 2014

Permission is granted to copy, distribute and/or modify this document
under the terms of the GNU Free Documentation License, Version 1.3
or any later version published by the Free Software Foundation;
with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.
A copy of the license is included in the file entitled "GNU
Free Documentation License" distributed with this document.


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

The template inserts the data named `page-title` inside the HTML `<title>` element and `page-head` in the `<h1>` element. It checks if `posts` is *not* true and prints a message telling the user there were no posts if so. Next, it checks if `posts` *is* true and prints them out if so. Printing involves outputting a `<table>` element, followed by two `<tr>`s and three `<td>`s for each element in an array and completed by closing the `<table>` tag. Finally, it closes the `<body>` and `<html>` tags.

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