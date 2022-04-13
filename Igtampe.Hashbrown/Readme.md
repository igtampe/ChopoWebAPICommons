# Hashbrown

Hashbrown is a very *very* simple hasher shortcut that uses ASP.NET's cryptography services. It also handles loading salt for the hasher, and can even generate one if none is found using System.Cryptography's services.

You can also mine salt with this hashbrown (somehow). It'll generate a 128bit Salt.

Once created, it only has one function. ***hashing***. It'll hash with SHA256, at 1000 itterations. 


<sub>~~haha I don't really know the words I'm saying. All I know is that it works according to my tests. Please help~~</sub>
