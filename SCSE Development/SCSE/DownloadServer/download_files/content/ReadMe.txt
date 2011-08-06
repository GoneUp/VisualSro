You have to place the files exacly like they should be in pk2 !!!

NOTE:
*The downloadserver represents the version pool thast means you can only place version folders there.
*The dummy.lzma is required if the serverversion is higher than the clientversion and the version folder is empty or does not exist. So dont delete the dummy.lzma otherwise the launcher will return a Patch has not finished (2) error.
*Use _ instead of spaces
*You can not downgrade the client with the downloadserver (by SV.T) because the version will be patched at least.

Version Numbersytle
1.004 = 4
1.024 = 24
1.204 = 204
2.024 = 1024
...

Folder layout:
+ = Folder
- = File

Sample Sructure:
+content
	-dummy.lzma
	-ReadMe.txt
	+28
		-sro_client.exe
		+Media
			-type.txt
			+server_dep
				+silkroad
					+textdata
						-textuisystem.xt
	+29
		-silkroad.exe
		+Data
			-...
			-...
			+...