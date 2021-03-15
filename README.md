# IFC Validator

A win10 native UWP app for the validation of properties in IFC files. 
Powered by [bSDD OpenAPI](https://github.com/buildingSMART/bSDD)

Ifc.Validator is a project to analyze the property presence in IFC files to check whether the IFC is fill the requirements.

With the requirement of properties or propertySets selected from bSDD classification, it's define a part of convention BIM in a project. Use a four level structure Domain-Classification-PropertySet-Property compare with each IfcProduct entity presented in the IFC file. Take all Pset and Qto group exist to estimate the quality of the deliverable IFC files.

## Screenshots
 
![alt text](https://github.com/youshengCode/Ifc.Validator/blob/master/Images/step2.png)

![alt text](https://github.com/youshengCode/Ifc.Validator/blob/master/Images/step4.png)

## Background

For a long time, I search something to contribute the quality control for IFC files or a validator for verify IFC files by a way algorithm other than human visualization verification. Idea start with my post in October 2020, [To be standardized: extra efforts for entering the BIM processes. – BIM Mars](https://bimmars.com/to-be-standardized-extra-efforts-for-entering-the-bim-processes/).

Until the **bSDD 2021 Hackathon** with [new bSDD](https://www.buildingsmart.org/users/services/buildingsmart-data-dictionary/) Open API, I finally found the right tool to start this job.

It is still a beta version completed in one week, and I will continue work in this repo. If you want to make a contribution, please contact me. Any kind of contribution is welcome. 

## Download

Release V1.0 is available and free in Microsoft Store for all win 10 user. 

Feel free to rate this app or leave a comment.

## License

[MIT](https://github.com/youshengCode/Ifc.Validator/blob/master/LICENSE.md) © 2021 [Yousheng WANG](https://github.com/youshengCode)
