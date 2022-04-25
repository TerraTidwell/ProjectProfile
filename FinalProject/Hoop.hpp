//
//  Hoop.hpp
//  FinalProject
//
//  Created by Spencer Westover on 9/22/21.
//

#include <stdio.h>
#include <SFML/Graphics.hpp>
//#pragma once


class Hoop{
public:

    Hoop(/* int x, int y*/){

        x_ = 700;
        y_ = 250;
        size_ = 100;
        
        
//        sf:: Texture hp;
//        hp.loadFromFile("Hoop.png");
//        shape_.setTexture(&hp);
        //hoop_.setOrigin(100,100);
        hoop_.setPosition(x_, y_);
        hoop_.setRadius(size_);
        hoop_.setFillColor(sf::Color:: White);
    
    }

    void drawHoop (sf::RenderWindow & window);
    void update(sf::RenderWindow & window, int & dirX, int & dirY);
    
   

//private:
    int x_, y_;
    float dx_, dy_;
    int size_;
    sf::CircleShape hoop_;


};

