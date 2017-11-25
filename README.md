# mmGRasl
The following program is a single frame gesture trainer for the Leap Motion Controller. Advanced, multi-frame gestures are not supported. This program is best used when recording and interpreting sign language.



System Requirements:
1. Windows 10
2. Visual Studios 2017
3. Leap Motion Controller


Bugs:
1. Sometimes multiple instances of the trainer will remain running even after closing the program. Use Task Manager to kill the program if this happens.



Getting Started:
1. Install Visual Studio 2017 with .Net 4.0 

2. Copy CODE\ directory to your desktop.
- Within CODE\ directory, double click on OpenProjectVS2017.sln to open the project in Visual Studios.

3. Within the INSTALLATION\ directory you will find the Leap SDK as well as a detailed Install Guide. Be sure to install the Leap Drivers to your PC, it is found within the SDK

4. Compile the code! However, do not use the application in debug mode within Visual Studios.
- Instead, hit the stop debugging button and run the code from the Build\ directory now located on your desktop.
- Run LeapMotionGestureTraining.exe found in Build\

5. You are now ready to begin using the application!



User Tutorial: RECORDING AND TRAINING GESTURES
1. When you first open the program you will find yourself on the Training Form. 
-In the top left corner you will have a dropdown MENU. There you will be able to navigate to Test or Interpretation Forms.

2. To train gestures first you must select either one of the built-in english letters (A is default) or click on the "Add Gesture" button to create a custom one.
- You can select the gesture you wish to train by double clicking on the name within the Gesture List. 
- At this point you must specify the Auto Capture Settings: 
-Time out= time between frames(500 ms default) I find 50 or 100 to be most effective however, this can bog down slower machines.
- File Count= number of frames you will capture in a single session. 40 is default to avoid hand fatigue however, the interpretation will not be accurate without at least a total of aprox 300 frames. More is better!
- Start Index= where the program begins counting from, I recommend leaving this at 1.
- If you wish to review the data recorded, simply enter the container of your selected gesture and click on the numbered .txt files.

3. Hit "Auto Capture" to activate the Leap Motion Controller.

4. Hit "Start" once you are ready to record.

5. When you have finished recording your gestures, it is now time to train the application. Hit the TRAIN button on the bottom and select the recorded gestures you wish to train.

6. All recorded gestures can be found within the Capture\ directory under its associated name. The data is a Well-formed JSON which can be coverted to a CSV for data analysis in excel.



User Tutorial: TESTING TRAINED GESTURES
1. Use the dropdown Menu to navigate to the Testing Form.

2. Hit the "Test" button to activate the Leap Motion Controller.

3. Place your hand over the device Field of View and perform the gesture you wish to test.

4. Hit the "Capture" button. The application will then tell you the Miss Rate of the performed gesture versus the learned recording.

5. You can now hit the Re-Capture button if you wish to test again.

6. Since the test data is saved, you might wish to delete the data post testing. If so, hit the "Remove all test data" button.



User Tutorial: INTERPRETATION MODE
1. After training and testing the accuracy of your recorded gesture, it is now time to show it off. Use the dropdown Menu to navigate to the Interpreter Form.

2. The Leap Motion Controller will be already active in this state. Simply perform your gesture and hit the "Interpret" button. The program will then try to decipher the gesture you performed and return it's name in the "Gesture Detected:" field.

3. If you are unsatisfied with the result, try again by hitting "Reinterpret".


Thank you, and I hope you enjoy playing around with this little program. If you are a developer intending on extending this program, review the DOCUMENTS\ directory to get a full understanding of the implemented machine learning algorithm.