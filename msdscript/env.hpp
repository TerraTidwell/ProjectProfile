//
//  env.hpp
//  msdscript
//
//  Created by Terra Tidwell on 3/21/22.
//

#ifndef env_hpp
#define env_hpp
#include "pointer.h"
#include "val.hpp"
#include <stdio.h>
#include <string>
#include <iostream>
#include <sstream>

class Val;

CLASS(Env) {
public:
    static PTR(Env) empty;
    virtual PTR(Val) lookup(std::string find_name) = 0;
    virtual bool equals(PTR(Env) e) = 0;
};

class EmptyEnv : public Env {
public:
    EmptyEnv();
    PTR(Val) lookup(std::string find_name);
    bool equals(PTR(Env) e);
};

class ExtendedEnv : public Env {
public:
    std::string name;
    PTR(Val) val;
    PTR(Env) rest;
    ExtendedEnv(std::string name, PTR(Val) val, PTR(Env) rest);
    PTR(Val) lookup(std::string find_name);
    bool equals(PTR(Env) e);
};



#endif /* env_hpp */
