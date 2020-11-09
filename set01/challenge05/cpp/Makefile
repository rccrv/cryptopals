ifeq ($(OS),Windows_NT)
	CC = clang++
	OBJECTS = main.obj
	EXECUTABLE = challenge05.exe
else
	CC = g++
	OBJECTS = main.o
	EXECUTABLE = challenge05
endif

all: $(OBJECTS)
	$(CC) $(OBJECTS) -o $(EXECUTABLE)

%.obj: %.cpp
	$(CC) -c $< -o $@
