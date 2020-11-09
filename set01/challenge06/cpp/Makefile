ifeq ($(OS),Windows_NT)
	CC = clang++
	OBJECTS = main.obj
	EXECUTABLE = challenge06.exe
else
	CC = g++
	OBJECTS = main.o
	EXECUTABLE = challenge05
endif

all: $(OBJECTS)
	$(CC) $(OBJECTS) -std=c++17 -o $(EXECUTABLE)

%.obj: %.cpp
	$(CC) -c $< -std=c++17 -o $@
