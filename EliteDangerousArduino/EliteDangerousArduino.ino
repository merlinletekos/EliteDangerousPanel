#include <Adafruit_NeoPixel.h>
#include <Adafruit_SSD1306.h>
#include <ArduinoJson.h>
#include <Joystick.h>

#define PIN 9
#define NUMPIXELS 10

#define HP_LED  0
#define LG_LED  1
#define ML_LED  2
#define CG_LED  3

#define OFF_LED  4
#define CHG_LED  5
#define ON_LED  6
#define CLD_LED  7

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

boolean off_led_satut;
boolean chg_led_satut;
boolean on_led_satut;
boolean cld_led_satut;

#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels

// Declaration for an SSD1306 display connected to I2C (SDA, SCL pins)
// The pins for I2C are defined by the Wire-library. 
#define OLED_RESET     -1 // Reset pin # (or -1 if sharing Arduino reset pin)
#define SCREEN_ADDRESS 0x3D ///< See datasheet for Address; 0x3D for 128x64, 0x3C for 128x32
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

void setup() {
  Serial.begin(9600);

  if(!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for(;;); // Don't proceed, loop forever
  }

  for (int i = 0; i < 2; i++) {
    pinMode(BUTTONS[i][0], INPUT_PULLUP);
    pinMode(BUTTONS[i][1], OUTPUT);
  }

  currentLocation();

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
      // Status
      else if (event_name == "FsdChargingEvent") {
        chg_led_satut = json_doc["value"];
      }
      else if (event_name == "FsdJumpStatusEvent") {
        on_led_satut = json_doc["value"];
      }
      else if (event_name == "FsdCooldownEvent") {
        cld_led_satut = json_doc["value"];
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

  currentLocation();

  delay(500);
}

void setPixels() {
  // if (hp_led_satut == true) {
  //   pixels.setPixelColor(HP_LED, pixels.Color(0, 150, 150));
  // }
  // if (lg_led_satut == true) {
  //   pixels.setPixelColor(LG_LED, pixels.Color(0, 150, 150));
  // }
  // if (ml_led_satut == true) {
  //   pixels.setPixelColor(ML_LED, pixels.Color(0, 150, 150));
  // }
  // if (cg_led_satut == true) {
  //   pixels.setPixelColor(CG_LED, pixels.Color(0, 150, 150));
  // }
  // if (chg_led_satut == true) {
  //   pixels.setPixelColor(CHG_LED, pixels.Color(0, 150, 150));
  // }
  // if (cld_led_satut == true) {
  //   pixels.setPixelColor(CLD_LED, pixels.Color(0, 150, 150));
  // }
  // if (on_led_satut == true) {
  //   pixels.setPixelColor(ON_LED, pixels.Color(0, 150, 150));
  // }
  // if (on_led_satut == false && cld_led_satut == false && chg_led_satut == true) {
  //   pixels.setPixelColor(OFF_LED, pixels.Color(0, 150, 150));
  // }
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

void testdrawcircle(void) {
  display.clearDisplay();

  for(int16_t i=0; i<max(display.width(),display.height())/2; i+=2) {
    display.drawCircle(display.width()/2, display.height()/2, i, SSD1306_WHITE);
    display.display();
    delay(1);
  }

  delay(2000);
}

void currentLocation() {
  display.clearDisplay();

  display.setTextSize(2); // Draw 2X-scale text
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(10, 0);
  display.println(F("scroll"));
  display.display();      // Show initial text

}
