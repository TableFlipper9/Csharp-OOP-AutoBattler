# C# OOP AutoBattler
## About
Project involving OOP principles, made for the OOP course for the second year of study. I chose this project because game design using oop usually includes additional logic and more principles then we were thought at the course.
## Features
The game is a functional autobattler where you chose your heroes and after the round begins, they fight the enemies in each level. The main gameplay loop is simple, level up heros, purchase upgrades and use iteams to make your heroes stronger, fight and progress through levels until you lose. Some of the features implemented in the game include:
- Player interaction through keyboard and mouse
- Save creation and load locally
## OOP principles showcase
Inheritance: All units inherit from the abstract base class "Unit", achieving code reuse and a clear model structure
Encapsulation: Classes use private modifiers and setter/getters are provided as necessary when access is needed
Abstraction: The base class of Unit is an abstract class, and can be used as a common interface in order to adress all specific units in a field 
Polymorphism: Method overriding is used for the units in order to dictate different behaviour based on the type of ehro/enemy it is
