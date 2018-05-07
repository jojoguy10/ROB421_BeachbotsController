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

AccelStepper liftStepper(AccelStepper::DRIVER, Y_STEP_PIN, Y_DIR_PIN);

// the setup function runs once when you press reset or power the board
void setup() {
	pinMode(Y_ENABLE_PIN, OUTPUT);
	digitalWrite(Y_ENABLE_PIN, LOW);

	Serial.begin(9600);

}

// the loop function runs over and over again until power down or reset
void loop() {
  
}

void serialEvent() {

}