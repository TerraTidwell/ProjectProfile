//
//  Expr.hpp
//  msdscript
//
//  Created by Terra Tidwell on 1/16/22.
//

#ifndef Expr_hpp
#define Expr_hpp
#include <stdlib.h>
#include <string>
#include <stdio.h>
#include "pointer.h"
#include "env.hpp"
#include "step.hpp"
#include "Cont.hpp"

class Val;

typedef enum {
    print_none,
    print_add,
    print_group_add_mult,
   
} print_mode_t;



CLASS(Expr) {
public:
    
    virtual ~Expr() { };
    
    int var = 0;
    
    virtual bool equals(PTR(Expr)e) = 0;
    
    virtual PTR(Val) interp(PTR(Env) env) = 0;
    
    //virtual PTR(Expr) subst(std:: string var, PTR(Expr) e ) = 0;
    virtual void step_interp() = 0;
    
    virtual void print (std::ostream& output) = 0;
    
    std::string to_string();
    
    void pretty_print(std::ostream& output);
    
    virtual void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) = 0;
    
    std::string to_string_pretty();
    
    
};

class NumExpr : public Expr {
public:
  int rep;
    

    NumExpr(int val);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr)e);
    void print (std::ostream& output);
    
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class AddExpr : public Expr {
public:
    int val;
  PTR(Expr) lhs;
  PTR(Expr) rhs;
    
    AddExpr(PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class MultExpr : public Expr {
public:
    int val;
  PTR(Expr) lhs;
  PTR(Expr) rhs;
    
    MultExpr(PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class VarExpr : public Expr {
public:
    std::string name;
    
    VarExpr(std::string name);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
    
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};


class LetExpr : public Expr {
public:
    
    PTR(Expr) body;
    PTR(Expr) rhs;
    std::string lhs;
    
    LetExpr(std::string lhs,  PTR(Expr) rhs,  PTR(Expr) body);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
    
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
    
    
};

class BoolExpr : public Expr {
public:
    bool val;
    
    BoolExpr(bool val);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class IfExpr : public Expr {
public:
    PTR(Expr) test;
    PTR(Expr) then;
    PTR(Expr) else_;
    
    IfExpr(PTR(Expr) test, PTR(Expr) then, PTR(Expr) else_);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class EqExpr : public Expr {
public:
    PTR(Expr) lhs;
    PTR(Expr) rhs;
    
    EqExpr(PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class FunExpr : public Expr {
public:
    std::string formal_arg;
    PTR(Expr) body;
    
    FunExpr(std::string formal_arg, PTR(Expr) body);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std:: string var, PTR(Expr) e);
    void print (std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
};

class CallExpr : public Expr {
public:
    PTR(Expr)to_be_called;
    PTR(Expr)actual_arg;
    
    CallExpr(PTR(Expr)to_be_called, PTR(Expr)actual_arg);
    bool equals(PTR(Expr)other);
    PTR(Val) interp(PTR(Env) env);
    //PTR(Expr) subst(std::string var, PTR(Expr) e);
    void print(std::ostream& output);
   
    void pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside);
    void step_interp();
    
};
#endif /* Expr_hpp */
