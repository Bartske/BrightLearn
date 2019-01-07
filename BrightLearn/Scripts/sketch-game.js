// Sketch One

let parentLifeWidth;
let parentLifeHeight;

let run = true;

let parentBonusWidth;
let parentBonusHeight;

let parentTimeWidth;
let parentTimeHeight;

let mn = 0;
let sc = 0;

var Interval = window.setInterval(startWatch, 10);
Interval = window.setInterval(Clock, 1000);

function startWatch(){
    if (BonusTime != BonusMaxTime){
        BonusTime += 10;
    }
}

function Clock(){
    if (sc != 59)
    {
        sc+= 1;
    }
    else{
        sc = 0;
        mn += 1;
    }
}

var timerSketch = function (p) {

    p.windowResized = function() {
        parentTimeHeight = document.getElementById('timeHolder').clientHeight;
        parentTimeWidth = document.getElementById('timeHolder').clientWidth;
    
        var canvas = p.createCanvas(parentTimeWidth, parentTimeHeight);
        canvas.parent('timeHolder');
        canvas.style('display', 'block');

    }

    p.setup = function () {
        parentTimeHeight = document.getElementById('timeHolder').clientHeight;
        parentTimeWidth = document.getElementById('timeHolder').clientWidth;

        var canvas = p.createCanvas(parentTimeWidth, parentTimeHeight);
        canvas.parent('timeHolder');
        canvas.style('display', 'block');

        //console.log(parentTimeWidth / 2 + "-" + parentTimeHeight / 2);
    };

    p.draw = function () {
        if (run) {
            var canvas = p.createCanvas(parentTimeWidth, parentTimeHeight);
            canvas.parent('timeHolder');
            canvas.style('display', 'block');

            p.background('rgba(255, 255, 255, 0)');
            p.translate(parentTimeWidth / 2, parentTimeHeight / 2);

            //console.log(parentTimeWidth / 2 + "-" + parentTimeHeight / 2);
            p.textSize(30);
            p.fill(255);
            p.noStroke();

            if (sc < 10 && mn < 10) {
                p.text("0" + mn + ":0" + sc, -35, 10);
            }
            else if (sc < 10) {
                p.text(mn + ":0" + sc, -35, 10);
            }
            else if (mn < 10) {
                p.text("0" + mn + ":" + sc, -35, 10);
            }
            else {
                p.text(mn + ":" + sc, -35, 10);
            }
        }
    };
};
var myp5 = new p5(timerSketch, 'timeHolder');


var lifeSketch = function (p) { // p could be any variable name


    p.windowResized = function() {
        parentLifeHeight = document.getElementById('lifeHolder').clientHeight;
        parentLifeWidth = document.getElementById('lifeHolder').clientWidth;

        var canvas = p.createCanvas(parentLifeWidth, parentLifeHeight);
        canvas.parent('lifeHolder');
        canvas.style('display', 'block');
    }
    
    p.setup = function () {
        parentLifeHeight = document.getElementById('lifeHolder').clientHeight;
        parentLifeWidth = document.getElementById('lifeHolder').clientWidth;

        var canvas = p.createCanvas(parentLifeWidth, parentLifeHeight);
        canvas.parent('lifeHolder');
        canvas.style('display', 'block');

        p.angleMode(p.DEGREES);
    };

    p.draw = function () {
        if (run) {

        var canvas = p.createCanvas(parentLifeWidth, parentLifeHeight);
        canvas.parent('lifeHolder');
        canvas.style('display', 'block');
        p.background('rgba(255, 255, 255, 0)');
        p.translate(parentLifeWidth / 2, parentLifeHeight / 2);

        p.textSize(24);
        p.fill(255);
        p.text('LEVENS', -45, 10);

        p.rotate(-90);

        p.strokeWeight(0);
        p.fill('rgba(94, 154, 214, 0.7)');
        let DegLifes = 0;
        if (Lifes == 0) {
            DegLifes = p.map(1, 0, 1000000, 0, 360);
        }
        else {
            DegLifes = p.map(Lifes, 0, MaxLifes, 0, 360);
        }
        p.arc(0, 0, parentLifeWidth, parentLifeHeight, 0, DegLifes, p.PIE);
        }

    };
};
myp5 = new p5(lifeSketch, 'lifeHolder');

// Sketch Two
var bonusSketch = function (p) {

    p.windowResized = function() {
        parentBonusHeight = document.getElementById('bonusHolder').clientHeight;
        parentBonusWidth = document.getElementById('bonusHolder').clientWidth;

        var canvas = p.createCanvas(parentBonusWidth, parentBonusHeight);
        canvas.parent('bonusHolder');
        canvas.style('display', 'block');

    }
    
    p.setup = function () {
        parentBonusHeight = document.getElementById('bonusHolder').clientHeight;
        parentBonusWidth = document.getElementById('bonusHolder').clientWidth;
        
        var canvas = p.createCanvas(parentBonusWidth, parentBonusHeight);
        canvas.parent('bonusHolder');
        canvas.style('display', 'block');

        p.angleMode(p.DEGREES);
    };

    p.draw = function () {
        if (run) {
            var canvas = p.createCanvas(parentBonusWidth, parentBonusHeight);
            canvas.parent('bonusHolder');
            canvas.style('display', 'block');

            p.background('rgba(255, 255, 255, 0)');
            p.translate(parentLifeWidth / 2, parentLifeHeight / 2);

            p.textSize(24);
            p.fill(255);
            p.text('BONUS', -43, 8);

            p.rotate(-90);

            p.strokeWeight(0);
            p.stroke(255);
            p.fill('rgba(94, 154, 214, 0.7)');
            let DegBonusTime = p.map(BonusTime, 0, BonusMaxTime, 0, 360);
            p.arc(0, 0, parentLifeWidth, parentLifeHeight, 0, DegBonusTime, p.PIE);
        }
    };
};
myp5 = new p5(bonusSketch, 'bonusHolder');