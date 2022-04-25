//
//  main.cpp
//  FinalProject
//
//  Created by Spencer Westover and Terra Tidwell on 9/20/21.
//

#include <SFML/Graphics.hpp>
#include <math.h>
#include <iostream>
#include "Basketball.hpp"
#include <string>

using namespace std;

int main()
{
    //Create the main program window.
    sf::RenderWindow window(sf::VideoMode(1400, 800), "Swish");
    window.setFramerateLimit(60);
    
    //Background Image
    sf::Texture t;
        t.loadFromFile("Court.png");
    sf::Sprite s(t);
        s.scale(sf::Vector2f(1.16,1.2));
    
    //Setting up the font to print the text
    sf::Font MyFont;
    if (!MyFont.loadFromFile("ScoreFont.ttf"))
    {
        cout<< "ERROR";
    }
    
    
    // Create a shape to draw
    Basketball basketball;
    Hoop hoop;

    //applies to hoop movement
    int dirX= 0;
    int dirY= 0;
    int xDir = 0;
    int yDir = 0;
    bool xMax = false;
    
    int collidedCounter = 0;
    int shotCounter = 0;
    
    sf:: Text text;
        text.setFont(MyFont);
        text.setString("SWISH!");
        text.setCharacterSize(50);
        text.setFillColor(sf::Color::Black);
        text.setScale(3.f, 3.f);
        text.move(425.f, 75.f);
    
    sf:: Text youWin;
        youWin.setFont(MyFont);
        youWin.setString("You win!");
        youWin.setCharacterSize(50);
        youWin.setFillColor(sf::Color::Black);
        youWin.setScale(3.f, 3.f);
        youWin.move(300.f, 75.f);

    //Run the program as long as the main window is open.
    while (window.isOpen())
    {
        window.clear(sf::Color::Black);

        // Check all the window's events that were triggered since the last iteration of the loop
        sf::Event event;
    
        while (window.pollEvent(event))
        {
            // "close requested" event: we close the window
            if (event.type == sf::Event::Closed){
            window.close();
            }
        }
        // If mouse is left clicked, push the ball in that direction
        if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
            sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                
                //Gather x and y coordinates of the mouse click
                int yCoord = mousePos.y;
                int xCoord = mousePos.x;
                bool collision = false;
                bool wait = false;
                bool wait2 = false;
                shotCounter++;
                while (collision == false){
                    window.clear(sf::Color::Black);
                    window.draw(s);
                    hoop.update(window, dirX, dirY);
                    hoop.drawHoop(window);
                    basketball.update(window, xCoord, yCoord, xDir, yDir, xMax);
                    if (xMax == true){
                        xMax = false;
                        break;
                    }
                    basketball.drawBasketball(window);
                    collision = basketball.collided(hoop);
                    window.display();
                }
                
                if(basketball.collided(hoop)){
                    window.draw(text);
                    window.display();
                    while(wait == false){
                        if (sf::Mouse::isButtonPressed(sf::Mouse::Right))
                        wait = true;
                    }
                    collidedCounter++;
                    if (collidedCounter == 3) {
                        window.clear(sf::Color::Black);
                        window.draw(s);
                        window.draw(youWin);
                        string str;
                        str = to_string(shotCounter);
                        sf:: Text shots;
                            shots.setFont(MyFont);
                            shots.setString("Total Shots Taken: " + str);
                            shots.setCharacterSize(25);
                            shots.setFillColor(sf::Color::Black);
                            shots.setScale(3.f, 3.f);
                            shots.move(200.f, 300.f);
                        window.draw(shots);
                        window.display();
                        
                        while(wait2 == false){
                            if (sf::Mouse::isButtonPressed(sf::Mouse::Left)){
                            wait2 = true;
                            }
                        }
                        cout << shotCounter;
                        return 1;
                    }
                }
            window.clear(sf::Color::Black);
        }
        
        
        // end the current frame
        window.draw(s);
        basketball.initialPosition(window);
        hoop.update(window, dirX, dirY);
        hoop.drawHoop(window);
        window.display();
    }

  return 0;
}
