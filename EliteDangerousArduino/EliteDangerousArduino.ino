#include <Adafruit_NeoPixel.h>
#include <Adafruit_SSD1306.h>
#include <ArduinoJson.h>
#include <Joystick.h>

#define PIN 9
#define NUMPIXELS 10

struct EventMap {
  const char* name;
  int index;
};

Joystick_ Joystick(JOYSTICK_DEFAULT_REPORT_ID,JOYSTICK_TYPE_GAMEPAD,
  2, 0,                  // Button Count, Hat Switch Count
  false, false, false,     // X and Y, but no Z Axis
  false, false, false,   // No Rx, Ry, or Rz
  false, false,          // No rudder or throttle
  false, false, false);  // No accelerator, brake, or steering

int BUTTONS[2][3] = {
  {5, 4, 0},
  {7, 6, 1}
};

Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
StaticJsonDocument<512> json_doc;

enum LedState {
  HP_LED,
  LG_LED,
  ML_LED,
  CG_LED,
  OFF_LED,
  CHG_LED,
  ON_LED,
  CLD_LED
};

const int NUM_LEDS = 8;
int LED_PINS[NUM_LEDS] = {0, 1, 2, 3, 4, 5, 6, 7};

boolean led_states[NUM_LEDS];

const EventMap eventMapping[] = {
  {"HardPointStatus", HP_LED},
  {"LandingGearStatus", LG_LED},
  {"MassLockedStatus", ML_LED},
  {"CargoScoopStatus", CG_LED},
  {"FsdChargingEvent", CHG_LED},
  {"FsdJumpStatusEvent", ON_LED},
  {"FsdCooldownEvent", CLD_LED},
};

void setup() {
  Serial.begin(9600);
  setupButtons();
  Joystick.begin();
  pixels.begin();
}

void loop() {
  if (Serial.available() > 0) {
    pixels.clear();
    const auto deser_err = deserializeJson(json_doc, Serial);

    if (!deser_err) {
      String event_name = json_doc["name"];
      boolean led_status = json_doc["value"];
      updateLedState(event_name.c_str(), led_status);
    }
    setPixels();
  }
  for (int i = 0; i < 2; i++) {
    int currentStateButton = digitalRead(BUTTONS[i][0]);
    digitalWrite(BUTTONS[i][1], currentStateButton ? HIGH : LOW);
    Joystick.setButton(BUTTONS[i][2], currentStateButton);
  }
  delay(500);
}

void updateLedState(const char* event_name, boolean value) {
  for (const auto& mapping : eventMapping) {
    if (strcmp(event_name, mapping.name) == 0) {
      led_states[mapping.index] = value;
      break;
    }
  }
}

void setPixels() {
  for (int i = 0; i < NUM_LEDS; i++) {
    pixels.setPixelColor(LED_PINS[i], led_states[i] ? pixels.Color(0, 150, 150) : pixels.Color(0, 0, 0));
  }
  pixels.show();
}

void setupButtons() {
  for (int i = 0; i < 2; i++) {
    pinMode(BUTTONS[i][0], INPUT_PULLUP);
    pinMode(BUTTONS[i][1], OUTPUT);
  }
}