//
//  val.hpp
//  msdscript
//
//  Created by Terra Tidwell on 2/14/22.
//

#ifndef val_hpp
#define val_hpp
#include <string>
#include <stdio.h>
#include "Expr.hpp"
#include "pointer.h"
#include "env.hpp"

class Expr;
class Env;
class Cont;

CLASS (Val){
public:
    
    virtual bool equals( PTR(Val) v) = 0;
    virtual PTR(Val) add_to(PTR(Val) rhs) = 0;
    virtual PTR(Val) mult_to(PTR(Val) rhs) = 0;
    virtual void print(std::ostream& outstream) = 0;
    std::string to_string();
    virtual PTR(Expr) to_expr() = 0;
    virtual bool is_true() = 0;
    virtual PTR(Val) call(PTR(Val) actual_arg) = 0;
    virtual void call_step(PTR(Val) actual_arg_val, PTR(Cont) rest) = 0;
};

class NumVal: public Val {
public:
    int val;
    
    NumVal(int val);
    
    bool equals(PTR(Val) v);
    PTR(Val)add_to(PTR(Val) rhs);
    PTR(Val) mult_to(PTR(Val) rhs);
    void print(std::ostream& outstream);
    virtual PTR(Expr) to_expr();
    bool is_true();
    PTR(Val) call(PTR(Val) actual_arg);
    std::string to_string();
    void call_step(PTR(Val) actual_arg_val, PTR(Cont) rest);
};

class BoolVal: public Val {
public:
    bool val;
    
    BoolVal(bool val);
    bool equals(PTR(Val) v);
    PTR(Val) add_to(PTR(Val) rhs);
    PTR(Val) mult_to(PTR(Val) rhs);
    void print(std::ostream& outstream);
    virtual PTR(Expr) to_expr();
    bool is_true();
    PTR(Val) call(PTR(Val) actual_arg);
    std::string to_string();
    void call_step(PTR(Val) actual_arg_val, PTR(Cont) rest);
    
};

class FunVal: public Val {
public:
    std::string formal_arg;
    PTR(Expr) body;
    PTR(Env) env;
    
    FunVal(std::string formal_arg, PTR(Expr) body, PTR(Env) env);
    bool equals(PTR(Val) v);
    PTR(Val) add_to(PTR(Val) rhs);
    PTR(Val) mult_to(PTR(Val) rhs);
    void print(std::ostream& outstream);
    virtual PTR(Expr) to_expr();
    bool is_true();
    PTR(Val) call(PTR(Val) actual_arg);
    std::string to_string();
    void call_step(PTR(Val) actual_arg_val, PTR(Cont) rest);
};
#endif /* val_hpp */
