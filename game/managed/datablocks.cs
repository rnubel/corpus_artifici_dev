$managedDatablockSet = new SimSet() {
   canSaveDynamicFields = "1";
      setType = "Datablocks";

   new t2dImageMapDatablock(platformer_sprites_pixelizedImageMap) {
      imageName = "~/data/images/platformer_sprites_pixelized.png";
      imageMode = "CELL";
      frameCount = "-1";
      filterMode = "SMOOTH";
      filterPad = "1";
      preferPerf = "1";
      cellRowOrder = "1";
      cellOffsetX = "0";
      cellOffsetY = "0";
      cellStrideX = "0";
      cellStrideY = "0";
      cellCountX = "8";
      cellCountY = "9";
      cellWidth = "64";
      cellHeight = "64";
      preload = "1";
      allowUnload = "0";
      force16Bit = "0";
   };
   new t2dAnimationDatablock(basicIdle) {
      imageMap = "platformer_sprites_pixelizedImageMap";
      animationFrames = "0 1 2 3";
      animationTime = "0.666667";
      animationCycle = "1";
      randomStart = "0";
      startFrame = "0";
      animationPingPong = "0";
      animationReverse = "0";
   };
   new t2dAnimationDatablock(basicRun) {
      imageMap = "platformer_sprites_pixelizedImageMap";
      animationFrames = "4 5 6 7 8 9 10 11";
      animationTime = "0.571429";
      animationCycle = "1";
      randomStart = "0";
      startFrame = "0";
      animationPingPong = "0";
      animationReverse = "0";
   };
   new t2dAnimationDatablock(basicWalk) {
      imageMap = "platformer_sprites_pixelizedImageMap";
      animationFrames = "32 33 34 35 36 37 38 39";
      animationTime = "0.666667";
      animationCycle = "1";
      randomStart = "0";
      startFrame = "0";
      animationPingPong = "0";
      animationReverse = "0";
   };
   new t2dImageMapDatablock(tilemap_1ImageMap) {
      imageName = "~/data/images/tilemap_1.png";
      imageMode = "CELL";
      frameCount = "-1";
      filterMode = "SMOOTH";
      filterPad = "1";
      preferPerf = "1";
      cellRowOrder = "1";
      cellOffsetX = "0";
      cellOffsetY = "0";
      cellStrideX = "65";
      cellStrideY = "65";
      cellCountX = "-1";
      cellCountY = "-1";
      cellWidth = "64";
      cellHeight = "64";
      preload = "1";
      allowUnload = "0";
      force16Bit = "0";
   };
   new t2dImageMapDatablock(white_stoneImageMap1) {
      imageName = "~/data/images/white_stone.png";
      imageMode = "CELL";
      frameCount = "-1";
      filterMode = "SMOOTH";
      filterPad = "1";
      preferPerf = "1";
      cellRowOrder = "1";
      cellOffsetX = "0";
      cellOffsetY = "0";
      cellStrideX = "65";
      cellStrideY = "65";
      cellCountX = "-1";
      cellCountY = "-1";
      cellWidth = "64";
      cellHeight = "64";
      preload = "1";
      allowUnload = "0";
      force16Bit = "0";
   };
   new t2dImageMapDatablock(bg_blank_skyImageMap) {
      imageName = "~/data/images/bg_blank_sky.png";
      imageMode = "FULL";
      frameCount = "-1";
      filterMode = "SMOOTH";
      filterPad = "1";
      preferPerf = "1";
      cellRowOrder = "1";
      cellOffsetX = "0";
      cellOffsetY = "0";
      cellStrideX = "0";
      cellStrideY = "0";
      cellCountX = "-1";
      cellCountY = "-1";
      cellWidth = "0";
      cellHeight = "0";
      preload = "1";
      allowUnload = "0";
      force16Bit = "0";
   };
   new t2dImageMapDatablock(platformer_sprites_pixelized21ImageMap) {
      imageName = "~/data/images/platformer_sprites_pixelized21.png";
      imageMode = "CELL";
      frameCount = "-1";
      filterMode = "SMOOTH";
      filterPad = "1";
      preferPerf = "1";
      cellRowOrder = "1";
      cellOffsetX = "0";
      cellOffsetY = "0";
      cellStrideX = "0";
      cellStrideY = "0";
      cellCountX = "8";
      cellCountY = "9";
      cellWidth = "64";
      cellHeight = "64";
      preload = "1";
      allowUnload = "0";
      force16Bit = "0";
   };
   new t2dAnimationDatablock(basicBlockIdle) {
      imageMap = "platformer_sprites_pixelized21ImageMap";
      animationFrames = "0 1 2 3";
      animationTime = "0.666667";
      animationCycle = "1";
      randomStart = "0";
      startFrame = "0";
      animationPingPong = "0";
      animationReverse = "0";
   };
   new t2dAnimationDatablock(basicBlockWalk) {
      imageMap = "platformer_sprites_pixelized21ImageMap";
      animationFrames = "32 33 34 35 36 37 38 39";
      animationTime = "0.666667";
      animationCycle = "1";
      randomStart = "0";
      startFrame = "0";
      animationPingPong = "0";
      animationReverse = "0";
   };
   new t2dAnimationDatablock(basicAttack) {
      imageMap = "platformer_sprites_pixelizedImageMap";
      animationFrames = "26 27 28";
      animationTime = "0.75";
      animationCycle = "0";
      randomStart = "0";
      startFrame = "0";
      animationPingPong = "0";
      animationReverse = "0";
   };
};
