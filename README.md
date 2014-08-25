# Avatar Upload in MVC 5 #

### ASP.NET MVC 5 avatar upload implementation ###


This project is based on ASP.NET MVC 5 and can be used as an example of how to implement
user avatar uploading and croping. It uses jquery, jquery.form, and jcrop and it has some
extra code to manipulate the image before saving it locally.

The project contains an AvatarController.cs controller that contains the backend code
for receiving the uploaded file.

The Views/Avatar folder contain the main partial view that has three distinct parts - box
for selecting a file and showing the progress of uploading, a box for cropping the image,
and a box to display the result of the manipulation. All operations are done on one page (view)
making it smooth.

### Run the example ###

To try the functionality, just run the project, click on the Avatar Upload item in the navigation
bar (top menu), and use the buttons on the page to upload and crop an image.

The steps you should follow on the /Avatar/Upload page so you can see the example in action are:

1. Click Browse and select an image file.
2. Click Upload file button - a resized file is stored in /Temp folder on the server.
3. Use the left image to select a part of the image to crop. In this example the avatar is square.
4. Click Save avatar button - the cropped file is saved in /Avatars folder on the server.

Feel free to clone and modify the project for your needs. Look at alkl ToDo's in it so
you can customize the basic behaviour. The rest is up to you.

Good luck!