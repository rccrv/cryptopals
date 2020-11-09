fn hd(s1: &str, s2: &str) -> u32 {
    let r = s1
        .bytes()
        .zip(s2.bytes())
        .map(|(i, j)| (i ^ j) as u64)
        .fold(0, |hw, n| hw + n.count_ones());

    r
}

fn main() {
    let s1 = "this is a test";
    let s2 = "wokka wokka!!!";
    let r = hd(s1, s2);
    println!("{}", r);
}
