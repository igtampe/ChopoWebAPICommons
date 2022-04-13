# Controllers

Common abstract controllers that can handle a few reusable functions involving other objects in the IgtampeCommons.

## ErrorResultControllerBase
A base controller that has shortcuts to generate IActionResults using ErrorResults. This also includes a few shortcuts for NotFound() results involving objects, and a shortcut to generate a 418 (I'm Teapot).

## ErrorResult
An itty bitty object to return errors to a frontend that expects Json objects as a return from their requests.

## Image Controller:
An image controller to handle images from ChopoImageHandling.

|Method|Route|Description|
|-|-|-|
|GET|{ID}|Gets Image as file|
|GET|{ID}/info|Gets image info (an image object from ChopoImageHandling)|
|POST||Uploads a file to the DB|

## Notification Controller:
A notification controller for notifications from Notifier. Uses a ChopoSessionManager and ChopoAuth to determine notification ownership.

|Method|Route|Description|
|-|-|-|
|GET||Gets all notifications owned by session owner|
|DELETE|{ID}|Deletes notification|
|DELETE||Deletes all notifications owned by session owner|

## Users Controller:
Controller to handle authentication and session management. Uses a ChopoSessionManager and ChopoAuth.

|Method|Route|Description|
|-|-|-|
|GET|Dir|Gets directory of all users
|GET||Gets User tied to active session|
|GET|{ID}|Gets specified user|
|PUT||Using a ChangePasswordRequest, changes the user's password|
|PUT|{ID}/Reset|Resets a user's password. Requires executing session to be tied to admin user|
|PUT|Image|Using an ImageURL in the body, updates a user's profile picture's image url
|POST||Using a UserRequest, logs a user in|
|POST|Register|Using a UserRequest, registers a user|
|POST|Out|Logs a user out|
