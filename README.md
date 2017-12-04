# Avatar Upload in MVC 5 #

### ASP.NET MVC 5 avatar upload implementation ###


This project is based on ASP.NET MVC 5 and can be used as an example of how to implement
user avatar uploading and cropping. It uses **jquery**, **jquery.form**, and **jcrop** and it has some
extra code to manipulate the image before saving it locally.

The project contains an **AvatarController.cs** controller class that contains the back-end code
for receiving the uploaded file and saving the final, cropped selection.

The **Views/Avatar** folder contains the main partial view that has three distinct parts - a box
for selecting a file and showing the upload progress, a box for cropping the image,
and a box for displaying the result of the manipulation. All operations are done on one page (view)
for a smooth, contained solution.

### Run the example ###

To try the functionality, just run the project, click on the Avatar Upload item in the navigation
bar (top menu), and use the buttons on the page to upload and crop an image.

The steps you should follow on the **/Avatar/Upload** page so you can see the example in action are:

1. Click "Browse" (or "Choose File" or whatever your browser flavour provides you with) and select an image file.
2. Click "Open" - a resized file is stored in a /Temp folder on the server (or locally).
3. Select a part of the image (on the left) to crop (in this example cropping is done according to a 1:1 
   aspect ratio, i.e. the avatar is square).  The cropped result (what the final avatar will look like) is 
   displayed and updated on the right side of the page as the selection changes.
4. Click the "Save Avatar" button - the cropped file is saved in the /Avatars folder on the server (locally).

### DISCLAIMER ###
This project is provided **as-is** as it's getting old. It counts on the support of whoever uses it. 
So, feel free to clone and modify it for your needs. 

Look at all the **ToDo**'s in it so you can make it run and customize its basic behaviour. The rest is up to you.

Good luck!
