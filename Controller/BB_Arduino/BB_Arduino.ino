/*
 Name:		BB_Arduino.ino
 Created:	5/7/2018 1:37:41 PM
 Author:	Joe Kelly
*/

#include <MultiStepper.h>
#include <AccelStepper.h>

#define X_STEP_PIN         54
#define X_DIR_PIN          55
#define X_ENABLE_PIN       38
#define X_MIN_PIN           3
#define X_MAX_PIN           2

#define Y_STEP_PIN         60
#define Y_DIR_PIN          61
#define Y_ENABLE_PIN       56
#define Y_MIN_PIN          14
#define Y_MAX_PIN          15

#define Z_STEP_PIN         46
#define Z_DIR_PIN          48
#define Z_ENABLE_PIN       62
#define Z_MIN_PIN          18
#define Z_MAX_PIN          19

#define E_STEP_PIN         26
#define E_DIR_PIN          28
#define E_ENABLE_PIN       24

#define Q_STEP_PIN         36
#define Q_DIR_PIN          34
#define Q_ENABLE_PIN       30

#define SDPOWER            -1
#define SDSS               53
#define LED_PIN            13

#define FAN_PIN            9

#define PS_ON_PIN          12
#define KILL_PIN           -1

#define HEATER_0_PIN       10
#define HEATER_1_PIN       8
#define TEMP_0_PIN          13   // ANALOG NUMBERING
#define TEMP_1_PIN          14   // ANALOG NUMBERING

// Stepper motors
AccelStepper liftStepper(AccelStepper::DRIVER, X_STEP_PIN, X_DIR_PIN);
AccelStepper shootStepper(AccelStepper::DRIVER, Y_STEP_PIN, Y_DIR_PIN);
AccelStepper turnStepper(AccelStepper::DRIVER, Z_STEP_PIN, Z_DIR_PIN);
bool Y_MIN_LIMIT, Y_MAX_LIMIT;

// Photoresistor
int photoPin = 9;
int photoValue = 0;
int PHOTO_THRESHOLD = 400;

// Serial variables
String data = "";
int i = 0;
bool stringComplete = false;

// State machine variables
int CURRENT_CASE = 0;
bool FIRE = false;

void setup() {
	pinMode(X_ENABLE_PIN, OUTPUT);
	pinMode(Y_ENABLE_PIN, OUTPUT);
	pinMode(Z_ENABLE_PIN, OUTPUT);
	digitalWrite(X_ENABLE_PIN, LOW);
	digitalWrite(Y_ENABLE_PIN, LOW);
	digitalWrite(Z_ENABLE_PIN, LOW);

	Serial.begin(9600);
	data.reserve(200);

	liftStepper.setMaxSpeed(300);
	liftStepper.setAcceleration(500);
	liftStepper.setSpeed(0);

	shootStepper.setMaxSpeed(1000);
	shootStepper.setAcceleration(5000);
	shootStepper.setSpeed(0);

	turnStepper.setMaxSpeed(1000);
	turnStepper.setAcceleration(5000);
	turnStepper.setSpeed(0);
}

void serialEvent() {
	while (Serial.available()) {
		char inChar = (char)Serial.read();
		data += inChar;
		if (inChar == '\n') {
			stringComplete = true;
		}
	}
}

void loop() {
	Y_MIN_LIMIT = digitalRead(Y_MIN_PIN);
	Y_MAX_LIMIT = digitalRead(Y_MAX_PIN);

	photoValue = analogRead(photoPin);

	// "Shoot" state machine
	switch (CURRENT_CASE) {
		// Run motor forward until limit
		case 0:
			shootStepper.setSpeed(-600);
			shootStepper.runSpeed();
			if (!Y_MAX_LIMIT) {
				shootStepper.move(2000);
				CURRENT_CASE++;
			}
			break;

			// Run motor backward halfway
		case 1:
			shootStepper.setSpeed(600);
			shootStepper.run();
			if (shootStepper.distanceToGo() == 0) {
				CURRENT_CASE++;
			}
			break;

			// When motor is in position, wait for photoresistor
		case 2:
			if (FIRE) {
				shootStepper.setSpeed(600);
				CURRENT_CASE++;
			}
			break;

			// Move motor back to minimum limit, then go back to case -1 (an impossible case)
		case 3:
			shootStepper.runSpeed();
			if (!Y_MIN_LIMIT) {
				FIRE = false;
				CURRENT_CASE = 0;
			}
			break;

		default:
			break;
	}

	if (stringComplete) {
		switch (data.charAt(0)) {
			case 'U':
				//UP
				liftStepper.setSpeed(data.substring(1).toInt());
				break;

			case 'D':
				//DOWN
				liftStepper.setSpeed(-data.substring(1).toInt());
				break;

			case 'L':
				//LEFT
				turnStepper.setSpeed(-data.substring(1).toInt());
				break;

			case 'R':
				//RIGHT
				turnStepper.setSpeed(data.substring(1).toInt());
				break;

			case 'F':
				//FIRE
				FIRE = true;
				break;

			case 'S':
				//STOP
				liftStepper.setSpeed(0);
				turnStepper.setSpeed(0);
				break;

			default:
				break;
		}
		data = "";
		stringComplete = false;
	}

	liftStepper.runSpeed();
	turnStepper.runSpeed();
}
