//
//  parse.hpp
//  msdscript
//
//  Created by Terra Tidwell on 2/7/22.
//

#ifndef parse_hpp
#define parse_hpp
#include "Expr.hpp"
#include <stdio.h>
#include "pointer.h"
#include "env.hpp"
#include "Cont.hpp"


PTR(Expr) parse_num(std::istream& in);
PTR(Expr)parse_variable(std::istream& in);
PTR(Expr) parse_expr(std::istream& in);
PTR(Expr)parse_let(std::istream& in);
PTR(Expr) parse_function(std::istream& in);
PTR(Expr)parse_addend(std::istream& in);
PTR(Expr) parse_multicand(std::istream& in);
std::string parse_keyword(std::istream &in, std:: string keyword);
PTR(Expr) parse_str(std::string s);
void consume(std::istream &in, int expect);
void skip_whitespace(std::istream &in);
PTR(Expr) parse_true(std::istream &in);
PTR(Expr) parse_false(std::istream &in);
PTR(Expr) parse_if(std::istream &in);
PTR(Expr) parse_eq(std::istream &in);
PTR(Expr) parse_comparg(std::istream &in);
PTR(Expr) parse_inner(std::istream &in);







#endif /* parse_hpp */
