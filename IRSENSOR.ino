
//int inputPin = 13;
int inputPin = A0;
int val = 0; //reading pin status


void setup() {

  Serial.begin(9600);
  
  pinMode(inputPin, INPUT);

}

void loop() {
 
//val= digitalRead(inputPin); digita
  val = analogRead(inputPin);

    Serial.println(val, DEC);

    delay(1000);

    

  }
