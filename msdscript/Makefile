INCS = cmdline.h catch.h Expr.h parse.h val.h pointer.h step.hpp Cont.hpp env.hpp

OBJS = main.cpp cmdline.cpp Expr.cpp parse.cpp val.cpp step.cpp Cont.cpp env.cpp tests.cpp

CXXFLAGS = --std=c++14 -O2

msdscript: $(OBJS)
	$(CXX) $(CXXFLAGS) -o msdscript $(OBJS)

main.o: main.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<

cmdline.o: cmdline.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<

Expr.o: Expr.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<
	
parse.o: parse.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<
	
val.o: val.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<
	
env.o: env.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<

step.o: step.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<
	
Cont.o: Cont.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<

tests.o: tests.cpp $(INCS)
	$(CXX) $(CXXFLAGS) -c $<

.PHONY: test
test: msdscript
	./msdscript --test
