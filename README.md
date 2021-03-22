# IFC Validator

**Ifc.Validator** is a UWP project to analyze the property presence in IFC files to check whether the IFC is fill the requirements.
Powered by [buildingSMART Data Dictionary - bSDD OpenAPI](https://github.com/buildingSMART/bSDD)

[IFC Validator in Microsoft Store](https://www.microsoft.com/store/productId/9PNB50VKN6JL) is available and free for all win 10 user. 

![alt text](https://github.com/youshengCode/Ifc.Validator/blob/master/Images/ValidatorLogo.png)

## Screenshots
 
![alt text](https://github.com/youshengCode/Ifc.Validator/blob/master/Images/step2.png)

![alt text](https://github.com/youshengCode/Ifc.Validator/blob/master/Images/step4.png)

## Background

For a long time, I have been searching something to do the quality control for IFC files and the validation to verify IFC files by a way of algorithm other than a human visual check. This idea starts with my post in October 2020, [To be standardized: extra efforts for entering the BIM processes. – BIM Mars](https://bimmars.com/to-be-standardized-extra-efforts-for-entering-the-bim-processes/).

Until the **bSDD 2021 Hackathon** with [new bSDD](https://www.buildingsmart.org/users/services/buildingsmart-data-dictionary/) Open API, I finally found the right tool to start this job. With the requirement of properties or propertySets selected from bSDD classification, it's define a part of convention BIM in a project. Use a four level structure Domain-Classification-PropertySet-Property compare with each IfcProduct entity presented in the IFC file. Take all Pset and Qto group exist to estimate the quality of the deliverable IFC files.

It is still a beta version completed in one week, and I will continue work in this repo. Any kind of contribution is welcome. 

More information: [IFC Validator, a simple tool for IFC quality control – BIM Mars](https://bimmars.com/ifc-validator/)

## Future improvements

- Use MVD to describe requirements
- Export sum report and export with entity detail
- Multi-domain support
- Sub-entity support
- Multi-thread support
- Multi-language support

## License

[MIT](https://github.com/youshengCode/Ifc.Validator/blob/master/LICENSE.md) © 2021 [Yousheng WANG](https://github.com/youshengCode)
