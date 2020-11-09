#include <cstdint>
#include <iostream>
#include <string_view>

using namespace std;

uint32_t hd(string_view s1, string_view s2) {
  uint32_t sum = 0;
  for (int i = 0; i < s1.length(); i++) {
    uint8_t n = s1[i] ^ s2[i];
    while (n) {
      n &= n - 1;
      sum += 1;
    }
  }

  return sum;
}

int main() {
  auto s1 = "this is a test";
  auto s2 = "wokka wokka!!!";
  auto r = hd(s1, s2);
  cout << r << '\n';
  return 0;
}
