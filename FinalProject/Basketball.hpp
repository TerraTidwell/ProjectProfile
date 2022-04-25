//
//  Basketball.hpp
//  FinalProject
//
//  Created by Spencer Westover on 9/22/21.
//


#include <stdio.h>
#include <SFML/Graphics.hpp>
#include <iostream>

#include "Hoop.hpp"



class Basketball{
public:
    
    Basketball( int x = 0, int y = 0, int size = 50 ){
        
        x_ = x;
        y_ = y;
        size_ = size;
        dx_ = 0;
        dy_ = 0;

        ball_.setFillColor(sf::Color(255, 140, 0));
        ball_.setOrigin(50,50);
        ball_.setPosition(x_, y_);
        ball_.setRadius(size_);

    }
    void initialPosition (sf::RenderWindow & window);
    void update(sf::RenderWindow & window, int xCoord, int yCoord, int & xDir, int & yDir, bool & xMax);
    void drawBasketball (sf:: RenderWindow & window);
    bool collided(const Hoop &hoop);
    
private:
    int size_;
    int x_, y_;
    float dx_, dy_;
    
    sf::CircleShape ball_;
    
};


