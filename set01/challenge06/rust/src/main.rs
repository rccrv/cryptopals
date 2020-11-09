fn hw(mut n: u8) -> u8 {
    let mut r = 0;

    while n > 0 {
        r += n & 1;
        n >>= 1;
    }

    r
}

fn hd(s1: &str, s2: &str) -> Result<u64, &'static str> {
    let mut sum = 0;

    if s1.len() != s2.len() {
        return Err("strings with different sizes")
    }

    for i in s1.bytes().zip(s2.bytes()) {
        sum += hw(i.0 ^ i.1) as u64;
    }

    Ok(sum)
}

fn main() {
    let s1 = "this is a test";
    let s2 = "wokka wokka!!!";
    println!("{}", hd(s1, s2).unwrap());
}
