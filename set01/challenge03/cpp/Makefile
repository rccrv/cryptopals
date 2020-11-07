ifeq ($(OS),Windows_NT)
	CC = clang++
	OBJECTS = main.obj
	EXECUTABLE = challenge03.exe
else
	CC = g++
	OBJECTS = main.o
	EXECUTABLE = challenge03
endif

all: $(OBJECTS)
	$(CC) $(OBJECTS) -o $(EXECUTABLE)

%.obj: %.cpp
	$(CC) -c $< -o $@
