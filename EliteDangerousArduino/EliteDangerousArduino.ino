#include <Adafruit_NeoPixel.h>
#include <ArduinoJson.h>
#include <Joystick.h>

#define PIN 9
#define NUMPIXELS 10

#define HP_LED  0
#define LG_LED  1
#define ML_LED  2
#define CG_LED  3

#define PIN_BUTTON_SPC  5
#define PIN_BUTTON_JMP  7

#define LED_BUTTON_SPC  4
#define LED_BUTTON_JMP  6

Joystick_ Joystick(JOYSTICK_DEFAULT_REPORT_ID,JOYSTICK_TYPE_GAMEPAD,
  2, 0,                  // Button Count, Hat Switch Count
  false, false, false,     // X and Y, but no Z Axis
  false, false, false,   // No Rx, Ry, or Rz
  false, false,          // No rudder or throttle
  false, false, false);  // No accelerator, brake, or steering

int BUTTONS[2][3] = {
  {PIN_BUTTON_SPC, LED_BUTTON_SPC, 0},
  {PIN_BUTTON_JMP, LED_BUTTON_JMP, 1}
};

Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
StaticJsonDocument<512> json_doc;

boolean hp_led_satut;
boolean lg_led_satut;
boolean ml_led_satut;
boolean cg_led_satut;

void setup() {
  Serial.begin(9600);

  for (int i = 0; i < 2; i++) {
    pinMode(BUTTONS[i][0], INPUT_PULLUP);
    pinMode(BUTTONS[i][1], OUTPUT);
  }

  Joystick.begin();
  pixels.begin();
}

void loop() {
  Serial.println(Serial.available());
  if (Serial.available() > 0) {
    pixels.clear();
    const auto deser_err = deserializeJson(json_doc, Serial);

    if (!deser_err) {
      String event_name = json_doc["name"];
      boolean led_status = json_doc["value"];

      if(event_name == "HardPointStatus") {
        hp_led_satut = json_doc["value"];
      }
      else if (event_name == "LandingGearStatus") {
        lg_led_satut = json_doc["value"];
      }
      else if (event_name == "MassLockedStatus") {
        ml_led_satut = json_doc["value"];
      }
      else if (event_name == "CargoScoopStatus") {
        cg_led_satut = json_doc["value"];
      }
    }
    setPixels();
  }

  for (int i = 0; i < 2; i++) {
    int currentStateButton = digitalRead(BUTTONS[i][0]);

    if (currentStateButton) {
      digitalWrite(BUTTONS[i][1], HIGH);
    }
    else {
      digitalWrite(BUTTONS[i][1], LOW);
    }
    Joystick.setButton(BUTTONS[i][2], currentStateButton);
  }

  delay(500);
}

void setPixels() {
  if (hp_led_satut == true) {
    pixels.setPixelColor(HP_LED, pixels.Color(0, 150, 150));
  }
  if (lg_led_satut == true) {
    pixels.setPixelColor(LG_LED, pixels.Color(0, 150, 150));
  }
  if (ml_led_satut == true) {
    pixels.setPixelColor(ML_LED, pixels.Color(0, 150, 150));
  }
  if (cg_led_satut == true) {
    pixels.setPixelColor(CG_LED, pixels.Color(0, 150, 150));
  }
  pixels.show();
}

void buttonBlinking() {
  for(int i = 0; i < 5; i++) {
    digitalWrite(BUTTONS[0][1], HIGH);
    digitalWrite(BUTTONS[1][1], HIGH);
    delay(50);
    digitalWrite(BUTTONS[0][1], LOW);
    digitalWrite(BUTTONS[1][1], LOW);
    delay(100);
    
  }
}
