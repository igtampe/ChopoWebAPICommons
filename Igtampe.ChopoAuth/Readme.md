# ChopoAuth
A small authentication package with a user object.

## User
A User object to store users in a DB. Uses Hashbrown for 

|Properties|Description|
|-|-|
|Username|Unique username for this user|
|Password|Hashed password of this user|
|ImageURL|ImageURL to the profile picture of this user|
|IsAdmin|Admin Role boolean. The only default Role in a User object|

|Methods|Description|
|-|-|
|UpdatePass()|Hashes and update the password of this user|
|CheckPass()|Hashes an attempt and checks it against the already hashed password of this user|
