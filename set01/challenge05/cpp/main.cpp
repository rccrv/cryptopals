#include <cstdint>
#include  <iomanip>
#include <iostream>
#include <sstream>
#include <sstream>
#include <string>
#include <vector>

using namespace std;

template <typename T1, typename T2>
struct cycle {
private:
  vector<T1> v;
  uint8_t counter;

public:
  cycle(T2 s) {
    v = {};
    counter = 0;
    for (T1 c : s) {
      v.push_back(c);
    }
  }

  T1 next() {
    T1 r;
    r = v.at(counter);
    counter = counter == 2 ? 0 : counter + 1;
    return r;
  }
};

int main() {
  string s = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
  cycle<char, string> iter("ICE");
  vector<uint8_t> bytes = {};
  stringstream r;

  for (char c : s) {
    bytes.push_back(static_cast<uint8_t>(c));
  }

  for (auto b : bytes) {
    r << std::setfill('0') << std::setw(2) << std::hex << (b ^ iter.next());
  }

  cout << r.str() << '\n';

  return 0;
}
