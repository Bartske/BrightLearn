let parentHeight;
let parentWidth;
var cX1;
var cX2;
var cY1;
var cY2;
var cR ;

let color = '#000000';

function drawCanvas() {
    parentWidth = document.getElementById('imgHolder').clientWidth;
    parentHeight = document.getElementById('imgHolder').clientHeight

    var myCanvas = createCanvas(parentWidth, parentHeight);
    myCanvas.parent('imgHolder');
    //console.log(parentWidth + " " + parentHeight);
}

function drawObject() {

    stroke(color);
    strokeWeight(4);
    noFill();
    
    cX1 = Math.floor((startX) / 1000 * parentWidth);
    cX2 = Math.floor((endX) / 1000 * parentWidth);
    cY1 = Math.floor((startY) / 1000 * parentHeight);
    cY2 = Math.floor((endY) / 1000 * parentHeight);
    cR = Math.floor((ratio) / 1000 * parentWidth);


    if (Rect) {
        rect(cX1, cY1, cX2, cY2);
    }
    else {
        ellipse(cX1, cY2, cR);
    }
}

function setup() {
    parentWidth = document.getElementById('imgHolder').clientWidth;
    parentHeight = document.getElementById('imgHolder').clientHeight
    drawCanvas();

}

function windowResized() {
    Reset();
}

function mouseClicked() {
    if(mouseX <= parentWidth && mouseX > 0 && mouseY > 0 && mouseY <= parentHeight){
        Reset();
    }
}

function mouseDragged() {
    if (mouseX <= parentWidth && mouseX > 0 && mouseY > 0 && mouseY <= parentHeight) {
        noFill();

        if (startX == null) {
            startX = mouseX;
            startY = mouseY;
        }
        else {
            drawCanvas();

            stroke(color);
            strokeWeight(4);


            if (Rect) {
                endX = mouseX - startX;
                endY = mouseY - startY;

                rect(startX, startY, endX, endY);

                //console.log(startX + "," + startY + " - " + endX + " , " + endY);
            }
            else {
                ratio = startX - mouseX;
                ellipse(startX, startY, ratio);

                //console.log(startX + "," + startY + " - " + ratio);
            }
        }
    }
}

function DrawColorChange() {
    color = document.getElementById('colorPicker').value;
}

function Reset() {
    drawCanvas();
    startX = null;
    startY = null;
    endX = null;
    endY = null;
    ratio = null;
}

function Rectangle() {
    Reset();
    Elps = false;
    Rect = true;
}

function Elipse() {
    Reset();
    Rect = false;
    Elps = true;
}
