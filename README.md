


<br>

[![Gardening Assistant](showcase/logo.png)](https://youtu.be/5G7V_HSD6M0)
> Pentru versiunea în română vezi: [README (RO)](README-RO.md)

# General Presentation

Gardening Assistant is a project made by Gabriela Burtan and Teodor Mihăescu in the 3rd
year of Computer Science College from Transylvania University of Brasov for Mobile Applications Development and Digital Image Processing subjects.

The application is a mobile assistant for maintenance and embellishment of the garden. It has the following functionalities: disease detection of plants, suggestions for garden decoration based on analyzed data.

The application is developed in Xamarin Forms Framework using C# (which interprets the project in Java and then compiles the native Android project).

> For more details you can watch:
> [Gardening Assistant - Demo video](https://youtu.be/5G7V_HSD6M0)

## User Interface

The graphic interface consist of the following views:

|<img src="showcase/homepage.png" alt="" width="140"><br>Main View|<img src="showcase/camera.png" alt="" width="140"><br>Camera View|<img src="showcase/selection_menu.png" alt="" width="140"><br>Selection View|
|:-:|:-:|:-:|
|<img src="showcase/selection_processing.png" alt="" width="140"><br>**Processing <br>Selection**|<img src="showcase/autobackground_alert.png" alt="" width="140"><br>**Auto-Background<br>Dialog Box**|<img src="showcase/autobackground_healthy.png" alt="" width="140"><br>**Healthy Leaf<br>Results**|
|<img src="showcase/results_blackspots.png" alt="" width="140"><br>**Blackspots Leaf<br>Results**|<img src="showcase/decorate.png" alt="" width="140"><br>**Decorate View**|<img src="showcase/plants.png" alt="" width="140"><br>**Plants View<br>(dummy)**|

## Images for testing

To test the application you can use the following images:

|<img src="showcase/leaf_blackspot_6.jpg" alt="" width="140"><br>Blackspots 1|<img src="showcase/leaf_blackspot_8.jfif" alt="" width="140"><br>Blackspots 2|<img src="showcase/leaf_blackspot_7.jfif" alt="" width="140"><br>Blackspots 3|
|:-:|:-:|:-:|
|<img src="showcase/leaf_clean_0.jpg" alt="" width="140"><br>**Healthy 1**|<img src="showcase/leaf_clean_1.jpg" alt="" width="140"><br>**Healthy 2**|<img src="showcase/leaf_clean_2.jpg" alt="" width="140"><br>**Healthy 3**|
|<img src="showcase/pot_1.jpg" alt="" width="140"><br>**Decorate 1**|<img src="showcase/img3.jfif" alt="" width="140"><br>**Decorate 2**|<img src="showcase/img13.jpg" alt="" width="140"><br>**Decorate 3**|

Images folder is available [here](https://drive.google.com/drive/folders/1Ab48aXJ17utoXJEorPpaw2ZbpZKiQxjn?usp=sharing)

## Project Structure

The project is structured as below.

There are three root projects: Xamarin.Forms, Xamarin.Android și Xamarin.iOS
<br>
<img src="showcase/structure_solution.png" alt="Solution Tree" width="">
<br>
Xamarin.Forms is the main project.

Xamarin.Forms project is structured as below:
<br>
<img src="showcase/structure_project.png" alt="Project Tree" width="">
<br>
Project's folders have the following functionalities:
- **Common**: Here are all the shared classes which have more than one dependency in the project, like the Dependency Injection container, enums, constants, etc.
- **Models**: Here are project's models, classes which handle structural transfer of data, and also image processing algorithms and their pipeline
- **Resources**: Here are some static resources
- **Services**: Here are services, classes which handle the business logic of the project like ImageManagerService, DialogBoxService, etc.
- **ViewModels**: Here are classes responsible with views logic and the bounded data 
- **Views**: Here are the views, UI pages 
- **App.xml**: Here is the main configuration file of the views
- **AppShell.xaml**: Here is the navigation layout responsible with navigation through pages because the project implements Shell.Navigation
- **AssemblyInfo.cs**: Here is information about the application, like: version, name, permissions. 

## Future improvements
- Implementation of functionalities for storage of plants data, creation of new plants, hints and details about current plants
- Data persistence using user account and data base
- Functionality for graphical schema generation and for specific statistics of a plant (health improvement)
- Detection of multiple diseases (see [10 Common Plant Diseases (and How to Treat Them)](https://www.familyhandyman.com/list/most-common-plant-diseases/))
- Algorithms for detection of brightness
- Machine learning algorithms for specie recognition
- Optimizations for filtering, denoisification and another pre-processing procedures
- Optimizations for image processing algorithms and selection algorithms
- Asynchronous processes
- Solving unhandled exceptions or bugs and displaying dialog box where is necessary

## Technologies and dependencies

The project is made using [Xamarin Forms](https://github.com/xamarin/Xamarin.Forms), with an Xamarin.Android project attached.

For selection part, bitmap manipulation and functionalities of image processing algorithms we used SkiaSharp.
For camera access and upload functionality we used camera module from XamarinCommunityToolkit framework.

For alerts and loading message box we used [Acr.UserDialogs](https://github.com/aritchie/userdialogs).

For dependency injection we used [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/DependencyInjection)..

For theme and visual style we used Material Design, available on [Xamarin.Forms.Visual.Material](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/visual/material-visual)

## Background removal functionality

For background removal and selected area extraction (selected manually by the user or automatically by the background removal algorithm) we used SkiaSharp framework and we implemented an algorithm which:

1. is touch-screen responsive

2. creates lists of points which synchronize their coordinates with image pixels

3. after the selection is done, an binarized image with black pixels on background and white pixels on selected area is created

4. the generated image at point 3 is overlay on the original image; thus, a new image will be created: this image will have pixels with
alpha channel 0 where binarized image has black pixels and pixels from original image where the binarized image has white pixels

5. thus, we have an image which has transparent background selected area from original image

This process can be seen below:

|<img src="showcase/selection_0.jpg" alt="" width="140"><br>Original <br>image|<img src="showcase/selection_1.png" alt="" width="140"><br>User<br> selection|<img src="showcase/selection_2.png" alt="" width="140"><br>Binarized<br>image|<img src="showcase/selection_3.png" alt="" width="140"><br>Result<br>image|
|:-:|:-:|:-:|:-:|

## Image Processing

For image processing part we implemented the following algorithms:

- Gaussian Filter

- Median Filter

- Mean Filter

- RGB - HSV conversion

- HSV - RGB conversion

- RGB - Grayscale conversion

- Euclidian distance between 2 colors

- Computing of Hue histogram and extraction of the maximum Hue value

- Otsu algorithm for binarization

- Color segmentation algorithm for auto background removal

- KMeanClustering algorithm for detection of the first 3 predominant colors

- Algorithm for computing complementary colors

<br>
From the above algorithm we used:

- RGB - HSV conversion

- HSV - RGB conversion

- RGB - Grayscale conversion

- Euclidian distance between 2 colors

- Otsu algorithm for binarization

- Color segmentation algorithm for auto background removal

- KMeanClustering algorithm for detection of the first 3 predominant colors

- Algorithm for computing complementary colors

## Auxiliary Links

- [Trello Board](https://trello.com/b/ncIVblHG/gardening-assistant)

- [Activity Journal](https://docs.google.com/document/d/1hEqILjdCqo6puv_jpFp1cqYhIiZxR2ykwIf-nIAsJfE/edit?usp=sharing)

- [Concept Diagrams - Image Processing](https://gardening-assistant-concepts.netlify.app/ip-concept.html)

- [Concept Diagrams - UX/UI](https://gardening-assistant-concepts.netlify.app/uxui-concept.html)

- [Documentation papers used](https://drive.google.com/drive/folders/1HV50YQ13YmZOf3d0m_IiwAi-vsUI-Xon?usp=sharing)

- [Image Resources Folder](https://drive.google.com/drive/folders/1Ab48aXJ17utoXJEorPpaw2ZbpZKiQxjn?usp=sharing)
