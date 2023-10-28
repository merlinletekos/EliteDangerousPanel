#include <Adafruit_NeoPixel.h>
#include <ArduinoJson.h>

#define PIN 3
#define NUMPIXELS 10

#define HP_LED  0
#define LG_LED  1

Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
StaticJsonDocument<512> json_doc;

boolean hp_led_satut;
boolean lg_led_satut;

void setup() {
  Serial.begin(9600);
  pixels.begin();
}

void loop() {
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
  }

  setPixels();
}

void setPixels() {
  if (hp_led_satut == true) {
    pixels.setPixelColor(HP_LED, pixels.Color(0, 150, 150));
  }
  if (lg_led_satut == true) {
    pixels.setPixelColor(LG_LED, pixels.Color(0, 150, 150));
  }
  pixels.show();
}
