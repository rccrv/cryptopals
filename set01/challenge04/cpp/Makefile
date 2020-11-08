ifeq ($(OS),Windows_NT)
	CC = clang++
	OBJECTS = main.obj
	EXECUTABLE = challenge04.exe
else
	CC = g++
	OBJECTS = main.o
	EXECUTABLE = challenge04
endif

all: $(OBJECTS)
	$(CC) $(OBJECTS) -o $(EXECUTABLE)

%.obj: %.cpp
	$(CC) -c $< -o $@
